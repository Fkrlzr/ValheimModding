# Bugfix Requirements Document

## Scope Note

The scope of this spec has been narrowed. The vegvisir interaction investigation (clauses 1.1, 1.2, 2.1, 2.2) is **deferred** and is not part of the current work. Those clauses are kept below for reference only and will be picked up in a later iteration. The **active scope** is the unlimited carry weight mod (clauses 1.3, 2.3, 3.5).

## Introduction

**Active:** The vanilla game marks the player as Encumbered when the inventory weight exceeds the maximum carry weight, which slows movement and drains stamina. The deliverable is a new BepInEx mod, in the same style as the existing mods (AutoParry, PickupRange, SteadyRegen) and in its own project folder in the workspace, that removes the carry weight limit so the player is never Encumbered.

**Deferred:** The player cannot interact with boss stone vegvisirs (for example Eikthyr's near spawn). Pressing the interact key does nothing, so the boss location is never revealed. The cause is not confirmed. A first read of the workspace mods' Harmony patches found none touching interaction or hover logic (PickupRange patches `Player.AutoPickup`, AutoParry patches `Character.RPC_Damage`, SteadyRegen patches `Player.UpdateFood`, `GuiBar.SetValue` and `ItemData.GetTooltip`). The investigation, including the possibility of a third-party plugin or an interaction between plugins, is postponed.

## Bug Analysis

### Current Behavior (Defect)

**Deferred:**

1.1 _(deferred)_ WHEN the player looks at a boss stone vegvisir (e.g. Eikthyr's near spawn) and presses the interact key THEN the system does not perform the vegvisir interaction and the boss location is not revealed on the map.

1.2 _(deferred)_ WHEN the player is within range of a vegvisir with the current set of installed mods THEN the system does not present the normal interaction hover prompt, or presents it but ignores the interaction (exact symptom to be confirmed during investigation).

**Active:**

1.3 WHEN the total weight of the player's inventory exceeds the maximum carry weight THEN the system marks the player as Encumbered, which slows movement and drains stamina.

### Expected Behavior (Correct)

**Deferred:**

2.1 _(deferred)_ WHEN the player looks at a boss stone vegvisir and presses the interact key THEN the system SHALL perform the vegvisir interaction and reveal the associated boss location on the map.

2.2 _(deferred)_ WHEN the player is within range of a vegvisir THEN the system SHALL show the normal hover prompt and respond to the interact key, with every workspace mod enabled.

**Active:**

2.3 WHEN the total weight of the player's inventory exceeds what would be the vanilla maximum carry weight THEN the system SHALL NOT mark the player as Encumbered, because the new mod makes the effective maximum carry weight effectively unlimited. The mod SHALL:

- live in its own project folder, following the PickupRange layout (`<Name>\<Name>\<Name>.csproj`, `<Name>.cs`, `Properties\AssemblyInfo.cs`), targeting .NET Framework 4.8 with `GamePath` defaulting to `E:\Game\Valheim` resolution as in PickupRange and a post-build copy to `BepInEx\plugins`;
- patch `Player.GetMaxCarryWeight` with a Harmony postfix that raises the returned value for the local player;
- expose a `1 - General` / `Enabled` config toggle (default on), consistent with the existing mods.

### Unchanged Behavior (Regression Prevention)

3.1 WHEN the player picks up items within auto pickup range THEN the system SHALL CONTINUE TO use the configured PickupRange behavior (extended range when enabled, vanilla range when disabled or toggled off).

3.2 WHEN a blockable hit lands on a player with a shield or blocking weapon THEN the system SHALL CONTINUE TO apply AutoParry behavior, including the existing config options and toggle.

3.3 WHEN the player has food equipped THEN the system SHALL CONTINUE TO apply SteadyRegen's smooth health regeneration, health bar display, and tooltip text.

3.4 _(deferred, applies once the vegvisir work resumes)_ WHEN the player interacts with other interactables (doors, chests, crafting stations, beds, portals, items) THEN the system SHALL CONTINUE TO interact normally.

3.5 WHEN the new carry weight mod is disabled in config (`Enabled` = false) THEN the system SHALL CONTINUE TO use the vanilla carry weight limit and Encumbered behavior.

3.6 WHEN the other third-party plugins in the BepInEx plugins folder are loaded THEN the system SHALL CONTINUE TO load and run them without changes from this fix.

## Bug Condition

```pascal
// Active scope: carry weight (clauses 1.3 / 2.3 / 3.5)
FUNCTION isBugCondition(X)
  INPUT: X of type Player (local player, with mod enabled)
  OUTPUT: boolean

  RETURN X.inventoryWeight > vanillaMaxCarryWeight(X)
END FUNCTION
```

```pascal
// Property: Fix Checking - Carry Weight
FOR ALL X WHERE isBugCondition(X) DO
  result ← GetMaxCarryWeight'(X)
  ASSERT result >= X.inventoryWeight AND NOT IsEncumbered'(X)
END FOR
```

```pascal
// Property: Preservation Checking
// mod disabled, or non-local player: behavior equals vanilla
FOR ALL X WHERE NOT isBugCondition(X) DO
  ASSERT GetMaxCarryWeight(X) = GetMaxCarryWeight'(X)  // when mod disabled
END FOR
```

```pascal
// Deferred scope: vegvisir interaction (clauses 1.1 / 1.2 / 2.1 / 2.2)
// Not part of the current work.
FUNCTION isBugCondition_vegvisir(X)
  INPUT: X of type PlayerAction
  OUTPUT: boolean
  RETURN X.target IS Vegvisir AND X.action = Interact
END FUNCTION
```
