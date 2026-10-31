# Implementation Plan

Scope: only the UnlimitedCarryWeight mod (clauses 1.3, 2.3, 3.5 and regression clauses 3.1 to 3.3, 3.6). The vegvisir work (1.1, 1.2, 2.1, 2.2, 3.4) is deferred and has no tasks here.

Testing approach: the game-dependent patch cannot run outside Valheim, so the decision logic is extracted to a small static helper, `CarryWeightLogic.ApplyLimit(bool enabled, bool isLocalPlayer, float vanilla)`, with no Unity or game types. Property-based tests target that helper in a separate test project (`UnlimitedCarryWeight\UnlimitedCarryWeight.Tests`). In-game checks are listed in the final checkpoint.

- [ ] 1. Write bug condition exploration test
  - **Property 1: Bug Condition** - Local player never Encumbered when mod enabled
  - **CRITICAL**: This test MUST FAIL on unfixed code - failure confirms the bug exists
  - **DO NOT attempt to fix the test or the code when it fails**
  - **NOTE**: This test encodes the expected behavior - it will validate the fix when it passes after implementation
  - **GOAL**: Surface counterexamples that demonstrate the bug exists
  - Create the test project `UnlimitedCarryWeight\UnlimitedCarryWeight.Tests` (.NET Framework 4.8, xUnit and FsCheck.Xunit, pinned versions) and link `CarryWeightLogic.cs` into it
  - Add `CarryWeightLogic.cs` with the UNFIXED baseline: `ApplyLimit` returns `vanilla` unchanged (mirrors vanilla `GetMaxCarryWeight`)
  - **Scoped PBT Approach**: scope the property to the bug condition: `enabled = true`, `isLocalPlayer = true`, random `vanilla` in [0, 100000], random `inventoryWeight` > `vanilla` (including 450 vs 300 and 5000 vs 300 from the design examples)
  - Assert from Expected Behavior: `ApplyLimit(true, true, vanilla) >= inventoryWeight` and therefore `inventoryWeight > result` is false (not Encumbered)
  - Run the test on UNFIXED code
  - **EXPECTED OUTCOME**: Test FAILS (this is correct - it proves the bug exists)
  - Document counterexamples (e.g. "ApplyLimit(true, true, 300) returns 300 while inventory is 450, so the player is Encumbered")
  - Mark task complete when the test is written, run, and the failure is documented
  - _Requirements: 1.3, 2.3_

- [ ] 2. Write preservation property tests (BEFORE implementing fix)
  - **Property 2: Preservation** - Vanilla carry weight when mod is off or player is remote
  - **IMPORTANT**: Follow observation-first methodology
  - Observe on UNFIXED code: `ApplyLimit(false, true, 300)` returns 300; `ApplyLimit(true, false, 300)` returns 300; `ApplyLimit(false, false, 1234.5f)` returns 1234.5
  - Write property-based tests: for all `vanilla` values, when `enabled = false` OR `isLocalPlayer = false`, `ApplyLimit(enabled, isLocalPlayer, vanilla) == vanilla` (non-bug-condition inputs, from Preservation Requirements)
  - Also cover the case with normal weight (inventory below the limit): result must not drop below `vanilla`
  - Run tests on UNFIXED code
  - **EXPECTED OUTCOME**: Tests PASS (this confirms baseline behavior to preserve)
  - Mark task complete when tests are written, run, and passing on unfixed code
  - _Requirements: 3.5, 3.1, 3.2, 3.3, 3.6_

- [ ] 3. Fix for Encumbered applied above the vanilla carry limit

  - [ ] 3.1 Create the UnlimitedCarryWeight project
    - Create `UnlimitedCarryWeight\UnlimitedCarryWeight\` mirroring `PickupRange\PickupRange\`
    - Copy `PickupRange.csproj` to `UnlimitedCarryWeight.csproj`; set a new `ProjectGuid`, `RootNamespace`, `AssemblyName` and `Compile Include` entries (`UnlimitedCarryWeight.cs`, `CarryWeightLogic.cs`, `Properties\AssemblyInfo.cs`)
    - Keep .NET Framework 4.8, Harmony 2.5.4, BepInEx and Unity references, and the publicized `assembly_valheim`, `assembly_utils`, `assembly_guiutils`
    - Keep the existing `GamePath` resolution and add a final fallback `E:\Game\Valheim` with `Condition="!Exists('$(GamePath)')"`
    - Keep the post-build `copy /Y` of `$(AssemblyName).dll` to `$(GamePath)\BepInEx\plugins`, honoring `SkipCopy`
    - Copy PickupRange's `Properties\AssemblyInfo.cs` and update title, product and GUID
    - _Bug_Condition: isBugCondition(input) where input == Player.m_localPlayer AND modEnabled AND inventory weight > vanillaMaxCarryWeight_
    - _Preservation: Preservation Requirements from design (other mods and plugins untouched, separate folder and DLL)_
    - _Requirements: 2.3, 3.6_

  - [ ] 3.2 Implement the decision helper
    - Replace the baseline stub in `CarryWeightLogic.cs` with: if `enabled && isLocalPlayer` return `Math.Max(vanilla, UnlimitedLimit)`, otherwise return `vanilla`
    - `UnlimitedLimit = 999999f` (bounded so the inventory UI does not render `float.MaxValue`); `Max` guarantees the result is never below vanilla
    - Keep the helper free of Unity and game types so the test project can compile it
    - _Bug_Condition: isBugCondition(input) from design_
    - _Expected_Behavior: expectedBehavior(result) from design - result >= inventory weight, not Encumbered_
    - _Preservation: result == vanilla when disabled or non-local_
    - _Requirements: 2.3, 3.5_

  - [ ] 3.3 Implement the plugin and Harmony patch
    - `UnlimitedCarryWeight : BaseUnityPlugin` with `ModName = "Unlimited Carry Weight"`, `ModVersion = "1.0.0"`, `ModGUID = "org.bepinex.plugins.unlimitedcarryweight"`
    - In `Awake`, bind config in section `1 - General`: `Enabled` (bool, default `true`, "If off, the vanilla carry weight limit is used.") and `Toggle key` (`KeyboardShortcut`, default `KeyCode.None`)
    - In `Update`, toggle `Enabled` on the shortcut and show a TopLeft message, same as PickupRange
    - Create `Harmony harmony = new(ModGUID); harmony.PatchAll(Assembly.GetExecutingAssembly());`
    - Add nested patch class `UnlimitedWeight` with `[HarmonyPatch(typeof(Player), nameof(Player.GetMaxCarryWeight))]` and a `Postfix(Player __instance, ref float __result)` that sets `__result = CarryWeightLogic.ApplyLimit(enabled.Value, __instance == Player.m_localPlayer, __result)`
    - Build in Release; a missing or renamed `Player.GetMaxCarryWeight` must fail the build
    - Confirm the DLL is copied to `E:\Game\Valheim\BepInEx\plugins`
    - _Bug_Condition: isBugCondition(input) from design_
    - _Expected_Behavior: expectedBehavior(result) from design_
    - _Preservation: Preservation Requirements from design_
    - _Requirements: 2.3, 3.5, 3.6_

  - [ ] 3.4 Verify bug condition exploration test now passes
    - **Property 1: Expected Behavior** - Local player never Encumbered when mod enabled
    - **IMPORTANT**: Re-run the SAME test from task 1 - do NOT write a new test
    - The test from task 1 encodes the expected behavior
    - When this test passes, it confirms the expected behavior is satisfied
    - Run the bug condition exploration test from task 1
    - **EXPECTED OUTCOME**: Test PASSES (confirms bug is fixed)
    - _Requirements: 2.3_

  - [ ] 3.5 Verify preservation tests still pass
    - **Property 2: Preservation** - Vanilla carry weight when mod is off or player is remote
    - **IMPORTANT**: Re-run the SAME tests from task 2 - do NOT write new tests
    - Run the preservation property tests from task 2
    - **EXPECTED OUTCOME**: Tests PASS (confirms no regressions)
    - Confirm all tests still pass after the fix (no regressions)
    - _Requirements: 3.5, 3.1, 3.2, 3.3, 3.6_

- [ ] 4. Checkpoint - Ensure all tests pass
  - Ensure all unit and property tests pass and the Release build succeeds
  - In-game checks (manual, run by the user): exceed the vanilla limit and confirm no Encumbered; set `Enabled` = false and confirm vanilla Encumbered returns; confirm PickupRange, AutoParry and SteadyRegen still behave the same; confirm the BepInEx log shows the plugin loaded with no patch errors; check whether another installed plugin also patches `GetMaxCarryWeight`
  - Ask the user if questions arise
