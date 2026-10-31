using HarmonyLib;

namespace SkToolbox;

// The game writes item quality as an unsigned 16 bit number whenever an item is saved to a world object (dropped items,
// item stands, armor stands ...), so any level above 65535 wraps around (70000 comes back as 4464). The inventory itself
// stores it as a full int and is not affected.
// To keep high levels intact without changing the save format, the real level is stashed in the item's custom data
// while it is being written, and restored when it is read back. Items without the entry load exactly as before, and
// clients without this mod simply see the wrapped value plus one unknown custom data entry.
internal static class SkItemQuality
{
	internal const string Key = "gex_quality";

	private const int UShortMax = 65535;

	[HarmonyPatch(typeof(ItemDrop.ItemData), "Save", typeof(ZPackage))]
	private static class PatchSave
	{
		private static void Prefix(ItemDrop.ItemData __instance)
		{
			if (__instance.m_customData == null)
			{
				return;
			}
			if (__instance.m_quality > UShortMax)
			{
				__instance.m_customData[Key] = __instance.m_quality.ToString();
			}
			else
			{
				__instance.m_customData.Remove(Key);
			}
		}

		private static void Postfix(ItemDrop.ItemData __instance)
		{
			// Keep the entry out of the in-memory item, it only exists for the duration of the write.
			__instance.m_customData?.Remove(Key);
		}
	}

	[HarmonyPatch(typeof(ItemDrop.ItemData), "Load", typeof(ZPackage), typeof(ItemDrop.ItemData), typeof(global::Version.Item))]
	private static class PatchLoad
	{
		private static void Postfix(ItemDrop.ItemData itemData)
		{
			if (itemData?.m_customData != null && itemData.m_customData.TryGetValue(Key, out var value))
			{
				if (int.TryParse(value, out var quality) && quality > UShortMax)
				{
					itemData.m_quality = quality;
				}
				itemData.m_customData.Remove(Key);
			}
		}
	}
}
