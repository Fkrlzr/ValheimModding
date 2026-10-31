# Unlimited Carry Weight Bugfix Design

## Overview

Vanilla Valheim marks the player as Encumbered when inventory weight exceeds `Player.GetMaxCarryWeight()`, slowing movement and draining stamina. The fix is a new BepInEx mod, `UnlimitedCarryWeight`, in its own project folder that mirrors the PickupRange layout. It applies a Harmony postfix to `Player.GetMaxCarryWeight` that replaces the result with a very large value for the local player, gated by a config toggle.

Scope note: the vegvisir interaction investigation (clauses 1.1, 1.2, 2.1, 2.2) is deferred. This design covers only clauses 1.3, 2.3 and 3.5, plus the regression clauses 3.1 to 3.3 and 3.6 for the other mods.

## Glossary

- **Bug_Condition (C)**: The local player's inventory weight exceeds the vanilla maximum carry weight, so vanilla marks them Encumbered.
- **Property (P)**: With the mod enabled, `GetMaxCarryWeight` returns a value at least as large as the inventory weight, so the player is not Encumbered.
- **Preservation**: With the mod disabled (or for non-local players), `GetMaxCarryWeight` returns exactly the vanilla value. Other mods are unaffected.
- **Player.GetMaxCarryWeight**: Method in `assembly_valheim` returning the player's carry limit (base value plus equipment and status effect modifiers).
- **Player.IsEncumbered / UpdateEncumber**: Vanilla logic that compares `m_inventory.GetTotalWeight()` with `GetMaxCarryWeight()` to apply the Encumbered status effect.
- **UnlimitedCarryWeight**: The new mod, GUID `org.bepinex.plugins.unlimitedcarryweight`.

## Bug Details

### Bug Condition

The bug manifests when the local player's inventory weight is greater than the vanilla max carry weight. `GetMaxCarryWeight` returns the vanilla limit, so the Encumbered status effect is applied.

**Formal Specification:**
```
FUNCTION isBugCondition(input)
  INPUT: input of type Player
  OUTPUT: boolean

  RETURN input == Player.m_localPlayer
         AND modEnabled
         AND input.inventory.GetTotalWeight() > vanillaMaxCarryWeight(input)
END FUNCTION
```

### Examples

- Base limit 300, inventory 450: vanilla is Encumbered. Expected: not Encumbered.
- Inventory 5000 (many stacks of ore): vanilla is Encumbered. Expected: not Encumbered.
- Inventory 100, limit 300: not Encumbered in either case. Expected: unchanged.
- Mod `Enabled` = false, inventory 450: Encumbered, as vanilla.

## Expected Behavior

### Preservation Requirements

**Unchanged Behaviors:**
- With `Enabled` = false, `GetMaxCarryWeight` returns the vanilla value and Encumbered behavior is vanilla (clause 3.5).
- PickupRange, AutoParry and SteadyRegen behavior is unchanged (clauses 3.1 to 3.3).
- Other third-party plugins load and run unchanged (clause 3.6).

**Scope:**
All inputs that do NOT involve the local player's carry limit while the mod is enabled should be unaffected. This includes:
- Other `Player` instances (remote players on a server)
- Any other method on `Player`
- Any state where `Enabled` is false

## Hypothesized Root Cause

This is a missing feature rather than a faulty code path, but the mechanism is:

1. **Vanilla limit is computed in one place**: `Player.GetMaxCarryWeight()` is the single source for the limit. Overriding its return value is enough to stop Encumbered from being applied.
2. **UI also reads the same method**: The inventory weight display shows `weight/max`. A huge value such as `float.MaxValue` would render poorly, so a bounded large constant is preferable.
3. **Risk, method signature drift**: If a game update changes the method name or signature, `PatchAll` would throw. This is verified at build time against the publicized assembly.
4. **Risk, other patches on the same method**: A third-party plugin may also patch `GetMaxCarryWeight`. A postfix with default priority runs after vanilla and may be overridden by later postfixes. Not confirmed, noted as a risk.

## Correctness Properties

Property 1: Bug Condition - Local player never Encumbered when mod enabled

_For any_ local player where the bug condition holds (inventory weight exceeds the vanilla max carry weight, mod enabled), the fixed `GetMaxCarryWeight` SHALL return a value greater than or equal to the inventory weight, so the player is not marked Encumbered.

**Validates: Requirements 2.3**

Property 2: Preservation - Vanilla carry weight when mod is off

_For any_ input where the bug condition does NOT hold (mod disabled, or a non-local player), the fixed `GetMaxCarryWeight` SHALL return the same value as the original method, preserving vanilla Encumbered behavior and the behavior of the other mods.

**Validates: Requirements 3.1, 3.2, 3.3, 3.5, 3.6**

## Fix Implementation

### Changes Required

**New project folder** (mirrors `PickupRange\PickupRange\`):
```
UnlimitedCarryWeight\
  UnlimitedCarryWeight\
    UnlimitedCarryWeight.csproj
    UnlimitedCarryWeight.cs
    Properties\AssemblyInfo.cs
```

**File**: `UnlimitedCarryWeight\UnlimitedCarryWeight\UnlimitedCarryWeight.csproj`

1. **Copy PickupRange.csproj** and change `ProjectGuid` (new GUID), `RootNamespace`, `AssemblyName` and the `Compile Include` entries. Keep `TargetFrameworkVersion` v4.8, Harmony 2.5.4 and BepInEx references, publicized `assembly_valheim`, `assembly_utils`, `assembly_guiutils`, and UnityEngine references.
2. **GamePath**: keep the existing registry/Steam-library resolution, and add a final fallback `E:\Game\Valheim` (`Condition="!Exists('$(GamePath)')"`) so the build resolves to the user's install even if the registry lookup fails.
3. **Post-build copy**: keep the `Copy` target (`copy /Y` of `$(AssemblyName).dll` to `$(GamePath)\BepInEx\plugins`), honoring `SkipCopy`.

**File**: `UnlimitedCarryWeight\UnlimitedCarryWeight\UnlimitedCarryWeight.cs`

**Class**: `UnlimitedCarryWeight : BaseUnityPlugin`

4. **Plugin metadata**: `ModName = "Unlimited Carry Weight"`, `ModVersion = "1.0.0"`, `ModGUID = "org.bepinex.plugins.unlimitedcarryweight"`.
5. **Config** in `Awake`, section `1 - General`, matching PickupRange:
   - `Enabled` (bool, default `true`): "If off, the vanilla carry weight limit is used."
   - `Toggle key` (`KeyboardShortcut`, default `KeyCode.None`): toggles `Enabled` in `Update` with a TopLeft message, same as PickupRange. Included for consistency with the existing mods.
6. **Harmony patch**: `Harmony harmony = new(ModGUID); harmony.PatchAll(Assembly.GetExecutingAssembly());`
7. **Postfix**:
   ```csharp
   [HarmonyPatch(typeof(Player), nameof(Player.GetMaxCarryWeight))]
   private static class UnlimitedWeight
   {
       private const float UnlimitedLimit = 999999f;

       [UsedImplicitly]
       private static void Postfix(Player __instance, ref float __result)
       {
           if (enabled.Value && __instance == Player.m_localPlayer)
           {
               __result = Mathf.Max(__result, UnlimitedLimit);
           }
       }
   }
   ```
   `Mathf.Max` guarantees the result is never lowered below vanilla.

**File**: `UnlimitedCarryWeight\UnlimitedCarryWeight\Properties\AssemblyInfo.cs`

8. **Copy PickupRange's AssemblyInfo.cs** with title, product and GUID updated.

## Testing Strategy

### Validation Approach

Surface the vanilla behavior first, then confirm the fix and preservation. The mod has no test project in the existing repo style, and the logic depends on the running game, so the checks are build verification plus in-game checks. The pure decision logic (enabled flag, local player check, `Max`) is small enough to unit test if extracted to a static helper, which is optional.

### Exploratory Bug Condition Checking

**Goal**: Confirm the root cause (single source of the limit) before relying on it.

**Test Plan**: Before enabling the mod, load a character, fill the inventory past the limit and confirm the Encumbered effect. Check the signature of `Player.GetMaxCarryWeight` in the publicized `assembly_valheim` (decompile or let the build confirm it).

**Test Cases**:
1. **Over limit, mod absent**: Inventory above limit shows Encumbered (confirms bug on unfixed code).
2. **Signature check**: Build fails if `Player.GetMaxCarryWeight` is missing or renamed.
3. **Other patches**: Check whether another installed plugin patches `GetMaxCarryWeight` (may change the outcome).

**Expected Counterexamples**:
- Encumbered status and reduced speed when weight exceeds the limit.
- Possible causes if the fix fails later: another plugin overriding the result after our postfix, or a game update changing the method.

### Fix Checking

**Pseudocode:**
```
FOR ALL input WHERE isBugCondition(input) DO
  result := GetMaxCarryWeight_fixed(input)
  ASSERT result >= input.inventory.GetTotalWeight() AND NOT IsEncumbered(input)
END FOR
```

### Preservation Checking

**Pseudocode:**
```
FOR ALL input WHERE NOT isBugCondition(input) DO
  ASSERT GetMaxCarryWeight_original(input) = GetMaxCarryWeight_fixed(input)
END FOR
```

**Testing Approach**: Property-based testing fits the extracted decision helper (random enabled flag, local or remote, random vanilla value, assert `result == vanilla` when disabled or remote and `result >= vanilla` otherwise). The in-game side is checked by hand.

**Test Cases**:
1. **Disabled preservation**: Set `Enabled` = false, confirm Encumbered returns at the vanilla limit.
2. **Other mods**: Confirm PickupRange, AutoParry and SteadyRegen still behave the same with the new DLL installed.
3. **Normal weight**: Below-limit inventory shows no change.

### Unit Tests

- Helper returns vanilla when disabled
- Helper returns vanilla for non-local players
- Helper returns at least the vanilla value, and the large constant, when enabled and local

### Property-Based Tests

- Random vanilla limits and enabled/local flags: result equals vanilla when off or remote, and is `>=` vanilla when on and local
- Random inventory weights up to the large constant: never Encumbered when on and local

### Integration Tests

- Build in Release and confirm the DLL is copied to `E:\Game\Valheim\BepInEx\plugins`
- In game: load a character, exceed the vanilla limit, confirm no Encumbered; toggle `Enabled` off and confirm vanilla behavior returns
- Confirm the BepInEx log shows the plugin loaded with no patch errors, alongside the other plugins
