using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Data;
using ServerDevcommands;
using Service;
using TMPro;
using UnityEngine;
using WorldEditCommands;

namespace InfinityHammer;

public class ObjectSelection : BaseSelection
{
	private int ZoopsX;

	private int ZoopsY;

	private int ZoopsZ;

	private Vector3 ZoopOffset;

	private Vector3 ZoopOrigin;

	private readonly Dictionary<Vector3Int, GameObject> Zoops = new Dictionary<Vector3Int, GameObject>();

	private readonly GameObject Wrapper;

	public List<SelectedObject> Objects = new List<SelectedObject>();

	public TerrainHeight? TerrainHeightInfo;

	public TerrainPaint? TerrainPaintInfo;

	private string SelectionBaseDescription = "";

	private Requirement[]? CachedResources;

	private bool ResourcesDirty = true;

	private GameObject BasePrefab
	{
		get
		{
			if (Zoops.Count > 0)
			{
				return Zoops.First().Value;
			}
			if (UsesSelectionRoot && Objects.Count == 1)
			{
				return Snapping.GetChildren(SelectedPrefab).FirstOrDefault() ?? SelectedPrefab;
			}
			return SelectedPrefab;
		}
	}

	public bool UsesSelectionRoot { get; private set; }

	private bool CanCollapseToSingleObject
	{
		get
		{
			if (Objects.Count == 1 && TerrainHeightInfo == null)
			{
				return TerrainPaintInfo == null;
			}
			return false;
		}
	}

	public void ZoopReset()
	{
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		foreach (GameObject item in Zoops.Values.ToList())
		{
			if (Object.op_Implicit((Object)(object)item))
			{
				RemoveObject(item);
			}
		}
		ZoopsX = 0;
		ZoopsY = 0;
		ZoopsZ = 0;
		ZoopOffset = default(Vector3);
		ZoopOrigin = default(Vector3);
		Zoops.Clear();
		ZoopPostprocess();
	}

	private void RemoveChildSub(Vector3Int index)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		RemoveObject(Zoops[index]);
		Zoops.Remove(index);
	}

	private void RemoveChildX()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; Math.Abs(i) <= Math.Abs(ZoopsY); i += Sign(ZoopsY))
		{
			for (int j = 0; Math.Abs(j) <= Math.Abs(ZoopsZ); j += Sign(ZoopsZ))
			{
				RemoveChildSub(new Vector3Int(ZoopsX, i, j));
			}
		}
	}

	private void RemoveChildY()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; Math.Abs(i) <= Math.Abs(ZoopsX); i += Sign(ZoopsX))
		{
			for (int j = 0; Math.Abs(j) <= Math.Abs(ZoopsZ); j += Sign(ZoopsZ))
			{
				RemoveChildSub(new Vector3Int(i, ZoopsY, j));
			}
		}
	}

	private void RemoveChildZ()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; Math.Abs(i) <= Math.Abs(ZoopsX); i += Sign(ZoopsX))
		{
			for (int j = 0; Math.Abs(j) <= Math.Abs(ZoopsY); j += Sign(ZoopsY))
			{
				RemoveChildSub(new Vector3Int(i, j, ZoopsZ));
			}
		}
	}

	private void AddChildSub(Vector3Int index)
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		GameObject basePrefab = BasePrefab;
		if (Zoops.Count == 0)
		{
			ZoopOrigin = (UsesSelectionRoot ? basePrefab.transform.localPosition : Vector3.zero);
		}
		Vector3 pos = ZoopOrigin + GetOffset(index);
		Zoops[index] = AddObject(basePrefab.GetComponent<ZNetView>(), pos);
		if (Configuration.ZoopMagic == "Mild")
		{
			Center();
		}
		if (Configuration.ZoopMagic == "Wild")
		{
			CenterOnRandomChild();
		}
	}

	private void CenterOnRandomChild()
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		List<GameObject> children = Snapping.GetChildren(SelectedPrefab);
		Vector3 localPosition = children[Random.Range(0, children.Count)].transform.localPosition;
		foreach (Transform item in SelectedPrefab.transform)
		{
			item.position -= localPosition;
		}
	}

	private void Center()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		List<GameObject> children = Snapping.GetChildren(SelectedPrefab);
		Bounds val = default(Bounds);
		foreach (GameObject item in children)
		{
			((Bounds)(ref val)).Encapsulate(item.transform.position);
		}
		Vector3 center = ((Bounds)(ref val)).center;
		foreach (Transform item2 in SelectedPrefab.transform)
		{
			item2.position -= center;
		}
	}

	private void AddChildX(string offset)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		UpdateOffsetX(offset);
		for (int i = 0; Math.Abs(i) <= Math.Abs(ZoopsY); i += Sign(ZoopsY))
		{
			for (int j = 0; Math.Abs(j) <= Math.Abs(ZoopsZ); j += Sign(ZoopsZ))
			{
				AddChildSub(new Vector3Int(ZoopsX, i, j));
			}
		}
	}

	private void AddChildY(string offset)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		UpdateOffsetY(offset);
		for (int i = 0; Math.Abs(i) <= Math.Abs(ZoopsX); i += Sign(ZoopsX))
		{
			for (int j = 0; Math.Abs(j) <= Math.Abs(ZoopsZ); j += Sign(ZoopsZ))
			{
				AddChildSub(new Vector3Int(i, ZoopsY, j));
			}
		}
	}

	private void AddChildZ(string offset)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		UpdateOffsetZ(offset);
		for (int i = 0; Math.Abs(i) <= Math.Abs(ZoopsX); i += Sign(ZoopsX))
		{
			for (int j = 0; Math.Abs(j) <= Math.Abs(ZoopsY); j += Sign(ZoopsY))
			{
				AddChildSub(new Vector3Int(i, j, ZoopsZ));
			}
		}
	}

	private Vector3 GetOffset(Vector3Int index)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		Vector3 zoopOffset = ZoopOffset;
		zoopOffset.x *= ((Vector3Int)(ref index)).x;
		zoopOffset.y *= ((Vector3Int)(ref index)).y;
		zoopOffset.z *= ((Vector3Int)(ref index)).z;
		return zoopOffset;
	}

	private void UpdateOffsetX(string offset)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		Vector3 val = HammerHelper.ParseSize(BasePrefab, offset);
		ZoopOffset.x = val.x * BasePrefab.transform.localScale.z;
	}

	private void UpdateOffsetY(string offset)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		Vector3 val = HammerHelper.ParseSize(BasePrefab, offset);
		ZoopOffset.y = val.y * BasePrefab.transform.localScale.y;
	}

	private void UpdateOffsetZ(string offset)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		Vector3 val = HammerHelper.ParseSize(BasePrefab, offset);
		ZoopOffset.z = val.z * BasePrefab.transform.localScale.z;
	}

	private void ZoopPostprocess()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		CountObjects();
		Vector3 vec = Scaling.Get().Vec3;
		Helper.GetPlayer().SetupPlacementGhost();
		Scaling.Set(vec);
	}

	public void ZoopRight(string offset)
	{
		if (Object.op_Implicit((Object)(object)BasePrefab) && (Objects.Count <= 1 || ZoopsX != 0 || ZoopsY != 0 || ZoopsZ != 0))
		{
			if (ZoopsX < 0)
			{
				RemoveChildX();
				ZoopsX++;
			}
			else
			{
				ZoopsX++;
				AddChildX(offset);
			}
			ZoopPostprocess();
		}
	}

	public void ZoopLeft(string offset)
	{
		if (Object.op_Implicit((Object)(object)BasePrefab) && (Objects.Count <= 1 || ZoopsX != 0 || ZoopsY != 0 || ZoopsZ != 0))
		{
			if (ZoopsX > 0)
			{
				RemoveChildX();
				ZoopsX--;
			}
			else
			{
				ZoopsX--;
				AddChildX(offset);
			}
			ZoopPostprocess();
		}
	}

	public void ZoopUp(string offset)
	{
		if (Object.op_Implicit((Object)(object)BasePrefab) && (Objects.Count <= 1 || ZoopsX != 0 || ZoopsY != 0 || ZoopsZ != 0))
		{
			if (ZoopsY < 0)
			{
				RemoveChildY();
				ZoopsY++;
			}
			else
			{
				ZoopsY++;
				AddChildY(offset);
			}
			ZoopPostprocess();
		}
	}

	public void ZoopDown(string offset)
	{
		if (Object.op_Implicit((Object)(object)BasePrefab) && (Objects.Count <= 1 || ZoopsX != 0 || ZoopsY != 0 || ZoopsZ != 0))
		{
			if (ZoopsY > 0)
			{
				RemoveChildY();
				ZoopsY--;
			}
			else
			{
				ZoopsY--;
				AddChildY(offset);
			}
			ZoopPostprocess();
		}
	}

	public void ZoopForward(string offset)
	{
		if (Object.op_Implicit((Object)(object)BasePrefab) && (Objects.Count <= 1 || ZoopsX != 0 || ZoopsY != 0 || ZoopsZ != 0))
		{
			if (ZoopsZ < 0)
			{
				RemoveChildZ();
				ZoopsZ++;
			}
			else
			{
				ZoopsZ++;
				AddChildZ(offset);
			}
			ZoopPostprocess();
		}
	}

	public void ZoopBackward(string offset)
	{
		if (Object.op_Implicit((Object)(object)BasePrefab) && (Objects.Count <= 1 || ZoopsX != 0 || ZoopsY != 0 || ZoopsZ != 0))
		{
			if (ZoopsZ > 0)
			{
				RemoveChildZ();
				ZoopsZ--;
			}
			else
			{
				ZoopsZ--;
				AddChildZ(offset);
			}
			ZoopPostprocess();
		}
	}

	private int Sign(int value)
	{
		if (value < 0)
		{
			return -1;
		}
		return 1;
	}

	public override void Destroy()
	{
		base.Destroy();
		Object.Destroy((Object)(object)Wrapper);
		Objects.Clear();
		TerrainHeightInfo = null;
		TerrainPaintInfo = null;
		SelectedPrefab = null;
	}

	private void SetTerrainState(TerrainHeight? terrainHeightInfo, TerrainPaint? terrainPaintInfo)
	{
		TerrainHeightInfo = terrainHeightInfo;
		TerrainPaintInfo = terrainPaintInfo;
	}

	public ObjectSelection(ZNetView view, bool singleUse, Vector3? scale, DataEntry? extraData, TerrainHeight? terrainHeightInfo = null, TerrainPaint? terrainPaintInfo = null)
	{
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Expected O, but got Unknown
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Expected O, but got Unknown
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		if (Object.op_Implicit((Object)(object)((Component)view).GetComponent<Player>()))
		{
			throw new InvalidOperationException("Players are not valid objects.");
		}
		Wrapper = new GameObject();
		Wrapper.SetActive(false);
		ZDO zDO = view.GetZDO();
		int prefab = ((zDO == null) ? ZDOKeys.Hash(view.GetPrefabName()) : zDO.GetPrefab());
		DataEntry data = (DataEntry)((zDO == null) ? ((object)extraData) : ((object)DataHelper.Merge((DataEntry[])(object)new DataEntry[2]
		{
			new DataEntry(zDO),
			extraData
		})));
		SingleUse = singleUse;
		SelectedPrefab = HammerHelper.SafeInstantiate(view, Wrapper);
		SelectedPrefab.transform.position = Vector3.zero;
		UpdateVisuals(SelectedPrefab, data);
		Objects.Add(new SelectedObject(prefab, IsScalable(view), data));
		if (zDO != null)
		{
			PlaceRotation.Set(SelectedPrefab);
		}
		SelectedPrefab.transform.rotation = Quaternion.identity;
		if (scale.HasValue)
		{
			SelectedPrefab.transform.localScale = scale.Value;
		}
		Scaling.Set(SelectedPrefab);
		bool flag = Snapping.GetSnapPoints(SelectedPrefab).Count > 0;
		if (Configuration.Snapping != "Off" && !flag)
		{
			Snapping.BuildSnaps(SelectedPrefab);
		}
		SetTerrainState(terrainHeightInfo, terrainPaintInfo);
		UpdateSelectionDescription();
		ResourcesDirty = true;
	}

	public ObjectSelection(Piece piece, bool singleUse)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Expected O, but got Unknown
		Wrapper = new GameObject();
		Wrapper.SetActive(false);
		ZNetView component = ((Component)piece).GetComponent<ZNetView>();
		int prefab = ZDOKeys.Hash(component.GetPrefabName());
		SelectedPrefab = Object.Instantiate<GameObject>(((Component)component).gameObject, Wrapper.transform);
		HammerHelper.DisableRandomMaterialValues(SelectedPrefab);
		((Object)SelectedPrefab).name = ((Object)component).name;
		SingleUse = singleUse;
		Objects.Add(new SelectedObject(prefab, IsScalable(component), null));
		Scaling.Set(SelectedPrefab);
	}

	public ObjectSelection(IEnumerable<ZNetView> views, bool singleUse, Vector3? scale, DataEntry? extraData, TerrainHeight? terrainHeightInfo = null, TerrainPaint? terrainPaintInfo = null)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Expected O, but got Unknown
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Expected O, but got Unknown
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		Wrapper = new GameObject();
		Wrapper.SetActive(false);
		UsesSelectionRoot = true;
		SingleUse = singleUse;
		SelectedPrefab = new GameObject();
		SelectedPrefab.transform.SetParent(Wrapper.transform);
		if (scale.HasValue)
		{
			SelectedPrefab.transform.localScale = scale.Value;
		}
		((Object)SelectedPrefab).name = $"Multiple ({views.Count()})";
		SelectedPrefab.transform.position = ((Component)views.First()).transform.position;
		foreach (ZNetView view in views)
		{
			DataEntry data = DataHelper.Merge((DataEntry[])(object)new DataEntry[2]
			{
				new DataEntry(view.GetZDO()),
				extraData
			});
			GameObject obj = HammerHelper.ChildInstantiate(view, SelectedPrefab);
			obj.transform.position = ((Component)view).transform.position;
			obj.transform.rotation = ((Component)view).transform.rotation;
			UpdateVisuals(obj, data);
			Objects.Add(new SelectedObject(view.GetZDO().GetPrefab(), IsScalable(view), data));
		}
		SelectedPrefab.transform.position = Vector3.zero;
		Snapping.GenerateSnapPoints(SelectedPrefab);
		CountObjects();
		PlaceRotation.Set(SelectedPrefab);
		Scaling.Set(SelectedPrefab);
		SetTerrainState(terrainHeightInfo, terrainPaintInfo);
		UpdateSelectionDescription();
		ResourcesDirty = true;
	}

	public ObjectSelection(Terminal terminal, Blueprint bp, Vector3 scale)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Expected O, but got Unknown
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Expected O, but got Unknown
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		Wrapper = new GameObject();
		Wrapper.SetActive(false);
		UsesSelectionRoot = true;
		SelectedPrefab = new GameObject();
		SelectedPrefab.transform.SetParent(Wrapper.transform);
		((Object)SelectedPrefab).name = bp.Name;
		SelectedPrefab.transform.localScale = scale;
		SelectedPrefab.transform.position = ((Component)Helper.GetPlayer()).transform.position;
		Piece val = SelectedPrefab.AddComponent<Piece>();
		val.m_name = bp.Name;
		val.m_description = bp.Description;
		if (val.m_description == "")
		{
			val.m_description = "Center: " + bp.CenterPiece;
		}
		SelectionBaseDescription = val.m_description;
		bool flag = false;
		foreach (BlueprintObject @object in bp.Objects)
		{
			if (@object.Prefab == bp.CenterPiece)
			{
				flag = true;
			}
			if (Configuration.UseBlueprintChance && @object.Chance != 1f && Random.value > @object.Chance)
			{
				continue;
			}
			try
			{
				GameObject prefab = ZNetScene.instance.GetPrefab(@object.Prefab);
				if (!Object.op_Implicit((Object)(object)prefab))
				{
					throw new InvalidOperationException("Prefab " + @object.Prefab + " not found.");
				}
				ZNetView component = prefab.GetComponent<ZNetView>();
				GameObject val2 = HammerHelper.ChildInstantiate(component, SelectedPrefab);
				val2.transform.localPosition = @object.Pos;
				val2.transform.localRotation = @object.Rot;
				val2.transform.localScale = @object.Scale;
				DataEntry data = (DataEntry)((@object.Data == null || @object.Data == "") ? ((object)ReadExtraInfo(val2, @object.ExtraInfo)) : ((object)DataHelper.Get(@object.Data)));
				UpdateVisuals(val2, data);
				Objects.Add(new SelectedObject(ZDOKeys.Hash(@object.Prefab), IsScalable(component), data));
			}
			catch (Exception ex)
			{
				HammerHelper.Message(terminal, "Warning: " + ex.Message);
			}
		}
		SetTerrainState(bp.TerrainHeight?.Clone(), bp.TerrainPaint?.Clone());
		if (CanCollapseToSingleObject)
		{
			ToSingle();
		}
		if (!flag)
		{
			if (bp.SnapPoints.Count == 0)
			{
				Snapping.GenerateSnapPoints(SelectedPrefab);
			}
			else
			{
				Snapping.CreateSnapPoints(SelectedPrefab, bp.SnapPoints);
			}
		}
		val.m_clipEverything = Snapping.CountSnapPoints(SelectedPrefab) == 0;
		Scaling.Set(SelectedPrefab);
		UpdateSelectionDescription();
	}

	private string BuildTerrainSummaryDescription()
	{
		List<string> list = new List<string>();
		if (TerrainHeightInfo != null)
		{
			list.Add("Terrain height radius: " + HammerHelper.Format(TerrainHeightInfo.GetRadius()));
		}
		if (TerrainPaintInfo != null)
		{
			list.Add("Terrain paint radius: " + HammerHelper.Format(TerrainPaintInfo.GetRadius()));
		}
		return string.Join("\n", list);
	}

	private static string BuildObjectCountDescription(Dictionary<int, int> counts)
	{
		KeyValuePair<int, int>[] array = counts.OrderBy((KeyValuePair<int, int> kvp) => kvp.Value).Reverse().ToArray();
		if (array.Length <= 5)
		{
			return string.Join("\n", array.Select((KeyValuePair<int, int> kvp) => $"{((Object)ZNetScene.instance.GetPrefab(kvp.Key)).name}: {kvp.Value}"));
		}
		return string.Join("\n", from kvp in array.Take(4)
			select $"{((Object)ZNetScene.instance.GetPrefab(kvp.Key)).name}: {kvp.Value}") + $"\n{array.Length - 4} other types: {array.Skip(4).Sum((KeyValuePair<int, int> kvp) => kvp.Value)}";
	}

	private void UpdateSelectionDescription()
	{
		Piece component = SelectedPrefab.GetComponent<Piece>();
		if (Object.op_Implicit((Object)(object)component))
		{
			string text = BuildTerrainSummaryDescription();
			List<string> list = (string.IsNullOrEmpty(SelectionBaseDescription) ? new List<string>() : SelectionBaseDescription.Split('\n').ToList());
			if (!string.IsNullOrEmpty(text))
			{
				list.Add(text);
			}
			component.m_description = string.Join("\n", list);
		}
	}

	public void Mirror()
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected O, but got Unknown
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		int num = 0;
		foreach (Transform item2 in SelectedPrefab.transform)
		{
			Transform val = item2;
			int item = ((num < Objects.Count) ? Objects[num].Prefab : 0);
			num++;
			if (Snapping.IsSnapPoint(((Component)val).gameObject))
			{
				item = 0;
				num--;
			}
			val.localPosition = new Vector3(val.localPosition.x, val.localPosition.y, 0f - val.localPosition.z);
			Vector3 localEulerAngles = val.localEulerAngles;
			((Vector3)(ref localEulerAngles))._002Ector(localEulerAngles.x, 0f - localEulerAngles.y, localEulerAngles.z);
			if (Configuration.MirrorFlip.Contains(item))
			{
				localEulerAngles.y += 180f;
			}
			val.localRotation = Quaternion.Euler(localEulerAngles);
		}
		Helper.GetPlayer().SetupPlacementGhost();
	}

	public void Postprocess()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		if (!UsesSelectionRoot && Snapping.CountSnapPoints(SelectedPrefab) == 0)
		{
			Snapping.CreateSnapPoint(SelectedPrefab, Vector3.zero, "Center");
		}
	}

	private static float Convert(int value, float defaultValue)
	{
		return value switch
		{
			0 => 0.1f, 
			1 => 0.5f, 
			2 => 1f, 
			_ => defaultValue, 
		};
	}

	private static void SetWear(WearNTear wearNTear, int wear)
	{
		if (wear != -1)
		{
			wearNTear.SetHealthVisual(Convert(wear, 1f), false);
		}
	}

	private static void SetGrowth(Plant plant, int growth)
	{
		if (growth != -1)
		{
			bool flag = growth == 0;
			bool flag2 = growth == 1;
			bool flag3 = growth == 2;
			bool flag4 = growth == 3;
			if (Object.op_Implicit((Object)(object)plant.m_healthyGrown))
			{
				plant.m_healthy.SetActive(flag);
				plant.m_unhealthy.SetActive(flag2);
				plant.m_healthyGrown.SetActive(flag3);
				plant.m_unhealthyGrown.SetActive(flag4);
			}
			else
			{
				plant.m_healthy.SetActive(flag || flag3);
				plant.m_unhealthy.SetActive(flag2 || flag4);
			}
		}
	}

	protected static DataEntry? ReadExtraInfo(GameObject obj, string extraInfo)
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected O, but got Unknown
		if (extraInfo == "")
		{
			return null;
		}
		if (extraInfo.StartsWith("\"") && extraInfo.EndsWith("\""))
		{
			extraInfo = Regex.Unescape(extraInfo.Substring(1, extraInfo.Length - 2));
		}
		DataEntry val = new DataEntry();
		Sign val2 = default(Sign);
		if (obj.TryGetComponent<Sign>(ref val2))
		{
			DataEntryExtensions.Set(val, ZDOVars.s_text, extraInfo);
			((TMP_Text)val2.m_textWidget).text = extraInfo;
		}
		if (Object.op_Implicit((Object)(object)obj.GetComponent<TeleportWorld>()))
		{
			DataEntryExtensions.Set(val, ZDOVars.s_tag, extraInfo);
		}
		if (Object.op_Implicit((Object)(object)obj.GetComponent<Tameable>()))
		{
			DataEntryExtensions.Set(val, ZDOVars.s_tamedName, extraInfo);
		}
		ItemDrop val3 = default(ItemDrop);
		Fish val4 = default(Fish);
		if (obj.TryGetComponent<ItemDrop>(ref val3) && !obj.TryGetComponent<Fish>(ref val4))
		{
			DataEntryExtensions.Set(val, ZDOVars.s_piece, 1);
		}
		ItemStand val5 = default(ItemStand);
		if (obj.TryGetComponent<ItemStand>(ref val5))
		{
			string[] array = extraInfo.Split(':');
			string text = array[0];
			int num = Parse.Int(array, 1, 0);
			int num2 = Parse.Int(array, 2, 1);
			int num3 = Parse.Int(array, 3, 0);
			DataEntryExtensions.Set(val, ZDOVars.s_item, ZDOKeys.Hash(text));
			DataEntryExtensions.Set(val, ZDOVars.s_variant, num);
			DataEntryExtensions.Set(val, ZDOVars.s_quality, num2);
			if (array.Length > 3)
			{
				DataEntryExtensions.Set(val, ZDOVars.s_type, num3);
			}
		}
		ArmorStand val6 = default(ArmorStand);
		if (obj.TryGetComponent<ArmorStand>(ref val6))
		{
			string[] array2 = extraInfo.Split(':');
			int num4 = Parse.Int(array2, 0, 0);
			DataEntryExtensions.Set(val, ZDOVars.s_pose, num4);
			for (int i = 0; i < val6.m_slots.Count; i++)
			{
				string text2 = Parse.String(array2, i * 2 + 2, "");
				int num5 = Parse.Int(array2, i * 2 + 3, 0);
				if (!(text2 == ""))
				{
					DataEntryExtensions.Set(val, ZDOKeys.Hash($"{i}_item"), ZDOKeys.Hash(text2));
					DataEntryExtensions.Set(val, ZDOKeys.Hash($"{i}_variant"), num5);
				}
			}
		}
		return val;
	}

	protected static void UpdateVisuals(GameObject obj, DataEntry? data)
	{
		if (data == null)
		{
			return;
		}
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		string text = default(string);
		Sign val = default(Sign);
		if (DataEntryExtensions.TryGetString(data, dictionary, ZDOVars.s_text, ref text) && obj.TryGetComponent<Sign>(ref val))
		{
			((TMP_Text)val.m_textWidget).text = text;
		}
		ItemStand val2 = default(ItemStand);
		if (obj.TryGetComponent<ItemStand>(ref val2) && HammerHelper.TryGetNameHash(data, ZDOVars.s_item, out var hash))
		{
			int num = default(int);
			int num2 = (DataEntryExtensions.TryGetInt(data, dictionary, ZDOVars.s_variant, ref num) ? num : 0);
			int num3 = default(int);
			int num4 = ((!DataEntryExtensions.TryGetInt(data, dictionary, ZDOVars.s_quality, ref num3)) ? 1 : num3);
			int num5 = default(int);
			int num6 = (DataEntryExtensions.TryGetInt(data, dictionary, ZDOVars.s_type, ref num5) ? num5 : 0);
			val2.SetVisualItem(hash, num2, num4, num6);
		}
		ArmorStand val3 = default(ArmorStand);
		if (obj.TryGetComponent<ArmorStand>(ref val3))
		{
			int num7 = default(int);
			val3.m_pose = (DataEntryExtensions.TryGetInt(data, dictionary, ZDOVars.s_pose, ref num7) ? num7 : 0);
			val3.m_poseAnimator.SetInteger("Pose", num7);
			bool setItemHack = SelectionPatches.SetItemHack;
			SelectionPatches.SetItemHack = true;
			try
			{
				int num8 = default(int);
				for (int i = 0; i < val3.m_slots.Count; i++)
				{
					hash = (HammerHelper.TryGetNameHash(data, ZDOKeys.Hash($"{i}_item"), out var hash2) ? hash2 : 0);
					int num9 = (DataEntryExtensions.TryGetInt(data, dictionary, ZDOKeys.Hash($"{i}_variant"), ref num8) ? num8 : 0);
					val3.SetVisualItem(i, hash, num9);
				}
			}
			finally
			{
				SelectionPatches.SetItemHack = setItemHack;
			}
		}
		Character character = default(Character);
		if (obj.TryGetComponent<Character>(ref character))
		{
			float num10 = default(float);
			if (DataEntryExtensions.TryGetFloat(data, dictionary, ZDOVars.s_health, ref num10))
			{
				DataEntryExtensions.Set(data, ZDOVars.s_maxHealth, num10);
				DataEntryExtensions.Set(data, ZDOVars.s_health, num10 * 1.000001f);
			}
			int num11 = default(int);
			LevelEffects val4 = default(LevelEffects);
			if (DataEntryExtensions.TryGetInt(data, dictionary, ZDOVars.s_level, ref num11) && num11 > 1 && obj.TryGetComponent<LevelEffects>(ref val4))
			{
				val4.m_character = character;
				val4.SetupLevelVisualization(num11);
			}
		}
		int wear = default(int);
		WearNTear wearNTear = default(WearNTear);
		if (DataEntryExtensions.TryGetInt(data, dictionary, Hashes.Wear, ref wear) && obj.TryGetComponent<WearNTear>(ref wearNTear))
		{
			SetWear(wearNTear, wear);
		}
		int growth = default(int);
		Plant plant = default(Plant);
		if (DataEntryExtensions.TryGetInt(data, dictionary, Hashes.Growth, ref growth) && obj.TryGetComponent<Plant>(ref plant))
		{
			SetGrowth(plant, growth);
		}
	}

	private void CountObjects()
	{
		if (Objects.Count >= 2)
		{
			((Object)SelectedPrefab).name = $"Multiple ({Snapping.CountActiveChildren(SelectedPrefab)})";
			Piece val = SelectedPrefab.GetComponent<Piece>();
			if (!Object.op_Implicit((Object)(object)val))
			{
				val = SelectedPrefab.AddComponent<Piece>();
			}
			val.m_clipEverything = Snapping.CountSnapPoints(SelectedPrefab) == 0;
			val.m_name = ((Object)SelectedPrefab).name;
			Dictionary<int, int> counts = (from obj in Objects
				group obj by obj.Prefab).ToDictionary((IGrouping<int, SelectedObject> kvp) => kvp.Key, (IGrouping<int, SelectedObject> kvp) => kvp.Count());
			SelectionBaseDescription = BuildObjectCountDescription(counts);
		}
	}

	public override DataEntry? GetData(int index = 0)
	{
		if (Objects.Count <= index)
		{
			throw new InvalidOperationException("Invalid index.");
		}
		return Objects[index].Data;
	}

	public override int GetPrefab(int index = 0)
	{
		if (Objects.Count <= index)
		{
			throw new InvalidOperationException("Invalid index.");
		}
		return Objects[index].Prefab;
	}

	public override bool IsScalingSupported()
	{
		return Objects.All((SelectedObject obj) => obj.Scalable);
	}

	public override GameObject GetPrefab(GameObject obj, Vector3 pos, Quaternion rot)
	{
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected O, but got Unknown
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		UndoHelper.BeginSubAction();
		if (!UsesSelectionRoot)
		{
			string prefabName = Utils.GetPrefabName(obj);
			Transform transform = HammerHelper.GetPlacementGhost().transform;
			ZDO val = DataHelper.Init(ZDOKeys.Hash(prefabName), pos, rot, (Vector3?)transform.lossyScale, GetData());
			if (val != null)
			{
				DungeonRooms.Reposition(val, transform);
			}
			return ZNetScene.instance.GetPrefab(prefabName);
		}
		GameObject val2 = new GameObject
		{
			name = "Blueprint"
		};
		val2.AddComponent<Piece>();
		return val2;
	}

	public override void AfterPlace(GameObject obj)
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		if (!UsesSelectionRoot)
		{
			ZNetView component = obj.GetComponent<ZNetView>();
			if (!Object.op_Implicit((Object)(object)component))
			{
				return;
			}
			Rigidbody body = component.m_body;
			if (body != null)
			{
				body.WakeUp();
			}
			BaseSelection.PostProcessPlaced(obj);
			ApplyTerrainChanges(obj.transform.position, obj.transform.rotation);
		}
		else
		{
			HandleMultiple(HammerHelper.GetPlacementGhost());
			Object.Destroy((Object)(object)obj);
		}
		UndoHelper.EndSubAction();
	}

	private void ApplyTerrainChanges(Vector3 placementPosition, Quaternion placementRotation)
	{
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Expected O, but got Unknown
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Expected O, but got Unknown
		//IL_03e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_039e: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0415: Unknown result type (might be due to invalid IL or missing references)
		//IL_0262: Unknown result type (might be due to invalid IL or missing references)
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_0265: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Expected O, but got Unknown
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		//IL_020f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Expected O, but got Unknown
		//IL_0290: Unknown result type (might be due to invalid IL or missing references)
		//IL_0292: Unknown result type (might be due to invalid IL or missing references)
		//IL_0293: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0301: Unknown result type (might be due to invalid IL or missing references)
		//IL_0306: Unknown result type (might be due to invalid IL or missing references)
		//IL_030b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0320: Expected O, but got Unknown
		//IL_0329: Unknown result type (might be due to invalid IL or missing references)
		//IL_032b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0342: Unknown result type (might be due to invalid IL or missing references)
		//IL_0347: Unknown result type (might be due to invalid IL or missing references)
		//IL_034f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0359: Unknown result type (might be due to invalid IL or missing references)
		//IL_035e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0363: Unknown result type (might be due to invalid IL or missing references)
		//IL_0378: Expected O, but got Unknown
		//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
		if (TerrainHeightInfo == null && TerrainPaintInfo == null)
		{
			return;
		}
		float num = TerrainInfo.ResolveRadius(TerrainHeightInfo, TerrainPaintInfo);
		Dictionary<Vector3, TerrainUndoData> dictionary = new Dictionary<Vector3, TerrainUndoData>();
		Dictionary<Vector3, TerrainUndoData> dictionary2 = new Dictionary<Vector3, TerrainUndoData>();
		Quaternion relativeRotation = ((TerrainHeightInfo == null) ? Quaternion.identity : GetRelativeYawRotation(placementRotation, TerrainHeightInfo.CenterRotation));
		Quaternion relativeRotation2 = ((TerrainPaintInfo == null) ? Quaternion.identity : GetRelativeYawRotation(placementRotation, TerrainPaintInfo.CenterRotation));
		TerrainComp[] compilers = Terrain.GetCompilers(placementPosition, new Range<float>(num));
		TerrainComp[] array = compilers;
		foreach (TerrainComp val in array)
		{
			int num2 = val.m_width + 1;
			TerrainUndoData val2 = new TerrainUndoData();
			TerrainUndoData val3 = new TerrainUndoData();
			for (int j = 0; j < num2; j++)
			{
				for (int k = 0; k < num2; k++)
				{
					Vector3 nodePos = VertexToWorld(val.m_hmap, j, k);
					int num3 = k * num2 + j;
					float? num4 = TerrainHeightInfo?.FindNearest(nodePos, placementPosition, relativeRotation);
					if (num4.HasValue && num3 < val.m_hmap.m_heights.Count)
					{
						float smoothWeight = TerrainHeightInfo.GetSmoothWeight(nodePos, placementPosition, relativeRotation, TerrainHeightInfo.Smooth);
						float num5 = val.m_hmap.m_heights[num3];
						float num6 = num4.Value + placementPosition.y;
						float num7 = Mathf.Lerp(num5, num6, smoothWeight);
						val2.Heights.Add(new HeightUndoData
						{
							Index = num3,
							Level = val.m_levelDelta[num3],
							Smooth = val.m_smoothDelta[num3],
							HeightModified = val.m_modifiedHeight[num3]
						});
						val.m_levelDelta[num3] += val.m_smoothDelta[num3] + num7 - num5;
						val.m_smoothDelta[num3] = 0f;
						val.m_modifiedHeight[num3] = val.m_levelDelta[num3] != 0f;
						val3.Heights.Add(new HeightUndoData
						{
							Index = num3,
							Level = val.m_levelDelta[num3],
							Smooth = val.m_smoothDelta[num3],
							HeightModified = val.m_modifiedHeight[num3]
						});
					}
					Color? val4 = TerrainPaintInfo?.FindNearest(nodePos, placementPosition, relativeRotation2);
					if (val4.HasValue && num3 < val.m_paintMask.Length)
					{
						float smoothWeight2 = TerrainPaintInfo.GetSmoothWeight(nodePos, placementPosition, relativeRotation2, TerrainPaintInfo.Smooth);
						Color val5 = val.m_paintMask[num3];
						Color val6 = Color.Lerp(val5, val4.Value, smoothWeight2);
						if (!val.m_modifiedPaint[num3] || val6 != val5)
						{
							val2.Paints.Add(new PaintUndoData
							{
								Index = num3,
								Paint = val.m_paintMask[num3],
								PaintModified = val.m_modifiedPaint[num3]
							});
							val.m_paintMask[num3] = val6;
							val.m_modifiedPaint[num3] = true;
							val3.Paints.Add(new PaintUndoData
							{
								Index = num3,
								Paint = val.m_paintMask[num3],
								PaintModified = val.m_modifiedPaint[num3]
							});
						}
					}
				}
			}
			dictionary[((Component)val).transform.position] = val2;
			dictionary2[((Component)val).transform.position] = val3;
		}
		if (dictionary.Count > 0 || dictionary2.Count > 0)
		{
			UndoHelper.AddTerrainAction(dictionary, dictionary2, placementPosition, num);
		}
		array = compilers;
		for (int i = 0; i < array.Length; i++)
		{
			Terrain.Save(array[i]);
		}
		ClutterSystem instance = ClutterSystem.instance;
		if (instance != null)
		{
			instance.ResetGrass(placementPosition, num);
		}
	}

	private static Quaternion GetRelativeYawRotation(Quaternion placementRotation, Quaternion originalRotation)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		Vector3 val = placementRotation * Quaternion.Inverse(originalRotation) * Vector3.forward;
		val.y = 0f;
		if (((Vector3)(ref val)).sqrMagnitude <= 0.0001f)
		{
			return Quaternion.identity;
		}
		return Quaternion.LookRotation(((Vector3)(ref val)).normalized, Vector3.up);
	}

	private static Vector3 VertexToWorld(Heightmap hmap, int x, int z)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		Vector3 position = ((Component)hmap).transform.position;
		position.x += (float)(x - hmap.m_width / 2) * hmap.m_scale;
		position.z += (float)(z - hmap.m_width / 2) * hmap.m_scale;
		return position;
	}

	private static bool IsScalable(ZNetView view)
	{
		if (view.m_syncInitialScale)
		{
			return true;
		}
		if (Configuration.ScaleZSyncObjects && Object.op_Implicit((Object)(object)((Component)view).GetComponent<ZSyncTransform>()))
		{
			return true;
		}
		return false;
	}

	private void HandleMultiple(GameObject ghost)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		ApplyTerrainChanges(ghost.transform.position, ghost.transform.rotation);
		List<GameObject> children = Snapping.GetChildren(ghost);
		for (int i = 0; i < children.Count; i++)
		{
			GameObject val = children[i];
			int prefab = GetPrefab(i);
			GameObject prefab2 = ZNetScene.instance.GetPrefab(prefab);
			if (Object.op_Implicit((Object)(object)prefab2))
			{
				ZDO val2 = DataHelper.Init(prefab, val.transform.position, val.transform.rotation, (Vector3?)val.transform.lossyScale, GetData(i));
				if (val2 != null)
				{
					DungeonRooms.Reposition(val2, val.transform);
				}
				BaseSelection.PostProcessPlaced(Object.Instantiate<GameObject>(prefab2, val.transform.position, val.transform.rotation));
			}
		}
	}

	public GameObject AddObject(ZNetView view, Vector3 pos)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		if (!UsesSelectionRoot)
		{
			ToMulti();
		}
		GameObject obj = HammerHelper.ChildInstantiate(view, SelectedPrefab);
		obj.transform.rotation = ((Component)view).transform.rotation;
		obj.transform.localPosition = pos;
		if (Configuration.Snapping != "Off")
		{
			Snapping.RegenerateSnapPoints(SelectedPrefab);
		}
		Objects.Add(new SelectedObject(Objects[0].Prefab, Objects[0].Scalable, Objects[0].Data));
		ResourcesDirty = true;
		return obj;
	}

	private void ToMulti()
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		GameObject selectedPrefab = SelectedPrefab;
		SelectedPrefab = new GameObject();
		SelectedPrefab.transform.SetParent(Wrapper.transform);
		SelectedPrefab.transform.position = selectedPrefab.transform.position;
		SelectedPrefab.transform.rotation = selectedPrefab.transform.rotation;
		selectedPrefab.transform.SetParent(SelectedPrefab.transform);
		selectedPrefab.transform.localScale = Vector3.one;
		Piece val = default(Piece);
		if (selectedPrefab.TryGetComponent<Piece>(ref val))
		{
			GameObject prefab = ZNetScene.instance.GetPrefab(Objects[0].Prefab);
			if (Object.op_Implicit((Object)(object)prefab) && !Object.op_Implicit((Object)(object)prefab.GetComponent<Piece>()))
			{
				Object.Destroy((Object)(object)val);
			}
		}
		UsesSelectionRoot = true;
	}

	public void RemoveObject(GameObject obj)
	{
		if (Objects.Count != 1)
		{
			obj.SetActive(false);
			obj.transform.SetParent((Transform)null);
			Object.Destroy((Object)(object)obj);
			Objects.RemoveAt(Objects.Count - 1);
			ResourcesDirty = true;
			if (CanCollapseToSingleObject)
			{
				ToSingle();
			}
			else if (Configuration.Snapping != "Off")
			{
				Snapping.RegenerateSnapPoints(SelectedPrefab);
			}
		}
	}

	private void ToSingle()
	{
		GameObject gameObject = ((Component)SelectedPrefab.transform.GetChild(0)).gameObject;
		HammerHelper.EnsurePiece(gameObject);
		gameObject.transform.SetParent(Wrapper.transform);
		SelectedPrefab.SetActive(false);
		Object.Destroy((Object)(object)SelectedPrefab);
		SelectedPrefab = gameObject;
		List<SelectedObject> list = new List<SelectedObject>();
		list.AddRange(Objects.Take(1));
		Objects = list;
		UsesSelectionRoot = false;
	}

	public override void Activate()
	{
		base.Activate();
		Scaling.Set(SelectedPrefab);
	}

	public override Piece GetSelectedPiece()
	{
		Piece selectedPiece = base.GetSelectedPiece();
		if (!Object.op_Implicit((Object)(object)selectedPiece) || !UsesSelectionRoot || !Configuration.GroupResourceCost)
		{
			return selectedPiece;
		}
		if (ResourcesDirty)
		{
			CachedResources = ResourceCost.Calculate(Objects);
			ResourcesDirty = false;
		}
		selectedPiece.m_resources = CachedResources;
		return selectedPiece;
	}
}
