using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MobBattle;

public sealed class BattleSide
{
	public int Id;
	public PickEntry Pick = null!;
	public int Stars;
	public ClllcChoice Clllc = new();
	public readonly List<ZDOID> Members = new();
	public readonly Dictionary<string, int> Composition = new();
	public int Alive;

	public int Initial;

	public string Name => Pick.Display;
}

public sealed class Battle
{
	public int Id;
	public BattleSide A = null!;
	public BattleSide B = null!;
	public float StartTime;
	public float EndTime;
	public bool Finished;
	public bool Stopped;
}

public static class BattleManager
{
	private static readonly int SideHash = "MobBattle_Side".GetStableHashCode();
	private static readonly int IdHash = "MobBattle_Id".GetStableHashCode();

	private static readonly Dictionary<Character, int> sideCache = new();
	private static readonly System.Random rng = new();
	private static readonly List<ZDOID> pendingCleanup = new();
	private static int nextBattleId = 1;
	private static float lastPoll;
	private static float lastRetarget;
	private static float lastPrune;
	private static float cleanupDeadline;

	public static Battle? Current { get; private set; }

	public static string LastResult { get; private set; } = "";

	public static bool Running => Current != null && !Current.Finished;

	// Which side (1 or 2) a creature fights for, or 0 when it is not part of a battle.
	// Reads the tag from the network object, so every client with the mod sees the same answer.
	public static int GetSide(Character? character)
	{
		if (character == null)
		{
			return 0;
		}

		if (sideCache.TryGetValue(character, out int cached))
		{
			return cached;
		}

		ZNetView? view = character.m_nview;
		if (view == null || !view.IsValid())
		{
			return 0;
		}

		int side = view.GetZDO().GetInt(SideHash, 0);
		sideCache[character] = side;
		return side;
	}

	public static bool Start(PickEntry a, PickEntry b, int countA, int countB, int starsA, int starsB, ClllcChoice? choiceA = null, ClllcChoice? choiceB = null)
	{
		Player? player = Player.m_localPlayer;
		if (player == null || ZNetScene.instance == null || ZoneSystem.instance == null)
		{
			return false;
		}

		Clear();

		Transform view = player.transform;
		Vector3 forward = Utils.DirectionXZ(GameCamera.instance != null ? GameCamera.instance.transform.forward : view.forward);
		if (forward == Vector3.zero)
		{
			forward = view.forward;
		}

		Vector3 right = Vector3.Cross(Vector3.up, forward).normalized;
		int columnsA = Mathf.Max(1, Mathf.CeilToInt(Mathf.Sqrt(countA)));
		int columnsB = Mathf.Max(1, Mathf.CeilToInt(Mathf.Sqrt(countB)));
		float spacing = 3f;
		// The bigger side decides how far apart the two groups start, so they never overlap.
		float half = Mathf.Max(8f, Mathf.Max(columnsA, columnsB) * spacing * 0.6f + 4f);
		Vector3 center = view.position + forward * MobBattle.spawnDistance.Value;

		Battle battle = new() { Id = nextBattleId++, StartTime = Time.time };
		battle.A = new BattleSide { Id = 1, Pick = a, Stars = starsA, Clllc = choiceA ?? new ClllcChoice() };
		battle.B = new BattleSide { Id = 2, Pick = b, Stars = starsB, Clllc = choiceB ?? new ClllcChoice() };

		SpawnSide(battle, battle.A, center - right * half, right, forward, right, countA, columnsA, spacing);
		SpawnSide(battle, battle.B, center + right * half, -right, forward, right, countB, columnsB, spacing);

		battle.A.Initial = battle.A.Members.Count;
		battle.B.Initial = battle.B.Members.Count;
		battle.A.Alive = battle.A.Initial;
		battle.B.Alive = battle.B.Initial;
		Current = battle;
		lastPoll = Time.time;
		lastRetarget = 0f;

		// Point every mob at the closest enemy so the fight starts right away.
		Retarget(true);

		if (battle.A.Initial == 0 || battle.B.Initial == 0)
		{
			LastResult = "Battle failed to start: one side had no creatures that could be spawned.";
			Clear();
			Current = null;
			return false;
		}

		player.Message(MessageHud.MessageType.Center, $"{battle.A.Name}  vs  {battle.B.Name}");
		return true;
	}

	private static void SpawnSide(Battle battle, BattleSide side, Vector3 sideCenter, Vector3 facing, Vector3 forward, Vector3 right, int count, int columns, float spacing)
	{
		int rows = Mathf.CeilToInt(count / (float)columns);
		for (int i = 0; i < count; i++)
		{
			MobEntry entry = side.Pick.Members[rng.Next(side.Pick.Members.Count)];
			GameObject? prefab = ZNetScene.instance.GetPrefab(entry.Prefab);
			if (prefab == null)
			{
				continue;
			}

			int row = i / columns;
			int column = i % columns;
			// Rows run along the line between the two sides, columns across it, centered on the side.
			Vector3 offset = forward * ((column - (columns - 1) * 0.5f) * spacing) + right * ((row - (rows - 1) * 0.5f) * spacing * 0.5f);
			Vector3 position = sideCenter + offset;
			position.y = ZoneSystem.instance.GetSolidHeight(position) + 0.5f;
			Quaternion rotation = Quaternion.LookRotation(facing);

			GameObject instance = UnityEngine.Object.Instantiate(prefab, position, rotation);
			Character? character = instance.GetComponent<Character>();
			ZNetView? view = instance.GetComponent<ZNetView>();
			if (character == null || view == null || !view.IsValid())
			{
				UnityEngine.Object.Destroy(instance);
				continue;
			}

			character.SetLevel(side.Stars + 1);
			// Boss kills normally unlock world progress, a staged fight must not.
			character.m_defeatSetGlobalKey = "";
			ClllcCompat.Prepare(character, side.Clllc);
			if (MobBattle.disableDrops.Value)
			{
				instance.GetComponent<CharacterDrop>()?.SetDropsEnabled(false);
			}

			ZDO zdo = view.GetZDO();
			zdo.Set(SideHash, side.Id);
			zdo.Set(IdHash, battle.Id);
			sideCache[character] = side.Id;

			MonsterAI? ai = instance.GetComponent<MonsterAI>();
			if (ai != null)
			{
				ai.Wakeup();
				ai.SetAlerted(true);
			}

			side.Members.Add(zdo.m_uid);
			side.Composition[entry.Display] = side.Composition.TryGetValue(entry.Display, out int n) ? n + 1 : 1;
		}
	}

	// Called every frame from the menu component.
	public static void Tick()
	{
		if (Time.time - lastPrune > 30f)
		{
			lastPrune = Time.time;
			foreach (Character? dead in sideCache.Keys.Where(c => c == null).ToList())
			{
				sideCache.Remove(dead!);
			}
		}

		if (Player.m_localPlayer == null)
		{
			Current = null;
			pendingCleanup.Clear();
			sideCache.Clear();
			return;
		}

		if (pendingCleanup.Count > 0 && Time.time - lastPoll > 0.5f)
		{
			DestroyPending();
		}

		Battle? battle = Current;
		if (battle == null || battle.Finished)
		{
			return;
		}

		if (Time.time - lastPoll < 0.5f)
		{
			return;
		}

		lastPoll = Time.time;
		battle.A.Alive = CountAlive(battle.A);
		battle.B.Alive = CountAlive(battle.B);

		if (battle.A.Alive == 0 || battle.B.Alive == 0)
		{
			Finish(battle, false);
			return;
		}

		if (Time.time - lastRetarget > 3f)
		{
			Retarget(false);
		}
	}

	private static int CountAlive(BattleSide side)
	{
		int alive = 0;
		if (ZDOMan.instance == null)
		{
			return side.Members.Count;
		}

		foreach (ZDOID id in side.Members)
		{
			ZDO zdo = ZDOMan.instance.GetZDO(id);
			if (zdo != null && zdo.GetFloat(ZDOVars.s_health, 1f) > 0f)
			{
				alive++;
			}
		}

		return alive;
	}

	// Mobs that lost or never had a target are pointed at the nearest living enemy.
	private static void Retarget(bool force)
	{
		lastRetarget = Time.time;
		Battle? battle = Current;
		if (battle == null)
		{
			return;
		}

		List<Character> sideA = LoadedAlive(battle.A);
		List<Character> sideB = LoadedAlive(battle.B);
		AssignTargets(sideA, sideB, force);
		AssignTargets(sideB, sideA, force);
	}

	private static void AssignTargets(List<Character> attackers, List<Character> enemies, bool force)
	{
		if (enemies.Count == 0)
		{
			return;
		}

		foreach (Character attacker in attackers)
		{
			MonsterAI? ai = attacker.GetComponent<MonsterAI>();
			if (ai == null || !attacker.m_nview.IsOwner())
			{
				continue;
			}

			if (!force && ai.m_targetCreature != null && !ai.m_targetCreature.IsDead())
			{
				continue;
			}

			if (ai.m_targetCreature != null && ai.m_targetCreature.IsDead())
			{
				ai.m_targetCreature = null;
			}

			Character? best = null;
			float bestDistance = float.MaxValue;
			foreach (Character enemy in enemies)
			{
				float distance = (enemy.transform.position - attacker.transform.position).sqrMagnitude;
				if (distance < bestDistance)
				{
					bestDistance = distance;
					best = enemy;
				}
			}

			if (best != null)
			{
				ai.SetAlerted(true);
				ai.SetTarget(best);
			}
		}
	}

	private static List<Character> LoadedAlive(BattleSide side)
	{
		List<Character> result = new();
		if (ZNetScene.instance == null)
		{
			return result;
		}

		foreach (ZDOID id in side.Members)
		{
			GameObject? go = ZNetScene.instance.FindInstance(id);
			Character? character = go != null ? go.GetComponent<Character>() : null;
			if (character != null && !character.IsDead())
			{
				result.Add(character);
			}
		}

		return result;
	}

	private static void Finish(Battle battle, bool stopped)
	{
		battle.Finished = true;
		battle.Stopped = stopped;
		battle.EndTime = Time.time;
		LastResult = BuildSummary(battle);
		if (Player.m_localPlayer != null)
		{
			string headline = stopped ? "Battle stopped" : battle.A.Alive == battle.B.Alive ? "Battle ended in a draw" : $"{(battle.A.Alive > battle.B.Alive ? battle.A.Name : battle.B.Name)} wins";
			Player.m_localPlayer.Message(MessageHud.MessageType.Center, headline);
		}
	}

	private static string BuildSummary(Battle battle)
	{
		float duration = (battle.Finished ? battle.EndTime : Time.time) - battle.StartTime;
		StringBuilder sb = new();
		if (battle.Stopped)
		{
			sb.AppendLine("Battle stopped early");
		}
		else if (battle.A.Alive == battle.B.Alive)
		{
			sb.AppendLine("Result: draw");
		}
		else
		{
			BattleSide winner = battle.A.Alive > battle.B.Alive ? battle.A : battle.B;
			BattleSide loser = winner == battle.A ? battle.B : battle.A;
			sb.AppendLine($"Winner: {winner.Name}  ({winner.Alive} of {winner.Initial} survived)");
			sb.AppendLine($"Defeated: {loser.Name}  (all {loser.Initial} fell)");
		}

		sb.AppendLine($"Duration: {duration:0} s");
		sb.AppendLine(Describe("Side A", battle.A));
		sb.AppendLine(Describe("Side B", battle.B));
		return sb.ToString().TrimEnd();
	}

	private static string Describe(string title, BattleSide side)
	{
		string stars = side.Stars > 0 ? $", {side.Stars} star{(side.Stars > 1 ? "s" : "")}" : "";
		string composition = string.Join(", ", side.Composition.OrderByDescending(p => p.Value).Select(p => $"{p.Value}x {p.Key}"));
		return $"{title}: {side.Name}{stars} | alive {side.Alive}/{side.Initial} | fell {side.Initial - side.Alive} | {composition}";
	}

	// Live status for the on-screen counter.
	public static string Status()
	{
		Battle? battle = Current;
		if (battle == null)
		{
			return "";
		}

		float time = (battle.Finished ? battle.EndTime : Time.time) - battle.StartTime;
		return $"{battle.A.Name}: {battle.A.Alive}/{battle.A.Initial}    vs    {battle.B.Name}: {battle.B.Alive}/{battle.B.Initial}    ({time:0} s)";
	}

	// Stops a running battle: records what was left, then removes every battle creature.
	public static void Stop()
	{
		Battle? battle = Current;
		if (battle != null && !battle.Finished)
		{
			battle.A.Alive = CountAlive(battle.A);
			battle.B.Alive = CountAlive(battle.B);
			Finish(battle, true);
		}

		Clear();
	}

	// Removes all battle creatures: the ones this session spawned and any loaded leftovers carrying a battle tag.
	public static void Clear()
	{
		if (ZNetScene.instance == null)
		{
			return;
		}

		Battle? battle = Current;
		if (battle != null)
		{
			pendingCleanup.AddRange(battle.A.Members);
			pendingCleanup.AddRange(battle.B.Members);
		}

		foreach (Character character in Character.GetAllCharacters().ToList())
		{
			if (character != null && GetSide(character) != 0)
			{
				ZDOID id = character.GetZDOID();
				if (!pendingCleanup.Contains(id))
				{
					pendingCleanup.Add(id);
				}
			}
		}

		cleanupDeadline = Time.time + 10f;
		DestroyPending();
	}

	private static void DestroyPending()
	{
		lastPoll = Time.time;
		if (ZNetScene.instance == null || ZDOMan.instance == null)
		{
			pendingCleanup.Clear();
			return;
		}

		for (int i = pendingCleanup.Count - 1; i >= 0; i--)
		{
			ZDOID id = pendingCleanup[i];
			GameObject? go = ZNetScene.instance.FindInstance(id);
			if (go != null)
			{
				ZNetView view = go.GetComponent<ZNetView>();
				if (view != null && view.IsValid())
				{
					if (view.IsOwner())
					{
						ZNetScene.instance.Destroy(go);
						pendingCleanup.RemoveAt(i);
					}
					else
					{
						view.ClaimOwnership();
					}

					continue;
				}
			}

			// Not loaded right now: take over the saved object and delete it.
			ZDO zdo = ZDOMan.instance.GetZDO(id);
			if (zdo == null)
			{
				pendingCleanup.RemoveAt(i);
				continue;
			}

			zdo.SetOwner(ZDOMan.GetSessionID());
			ZDOMan.instance.DestroyZDO(zdo);
			pendingCleanup.RemoveAt(i);
		}

		if (Time.time > cleanupDeadline)
		{
			pendingCleanup.Clear();
		}
	}
}
