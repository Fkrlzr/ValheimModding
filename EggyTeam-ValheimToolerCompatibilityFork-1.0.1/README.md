# ValheimToolerCompatibilityFork

Published and maintained as a compatibility fork by **EggyTeam**.

Unofficial compatibility fork of ValheimTooler, packaged as version 1.0.1.
Based on the ValheimTooler 1.11.2 distribution, with compatibility changes
targeting Valheim 1.0.12. This is not an official upstream release.

## Installation

Install with a Valheim mod manager and the listed BepInEx dependency.
For manual installation, extract the included BepInEx folder into the game folder.
Disable or uninstall other ValheimTooler packages first: this fork retains the
original plugin identity and must not be loaded alongside another copy.

## Controls

The default interface toggle on a fresh configuration is Delete.
Existing settings are preserved; check ToggleInterfaceKey in
BepInEx/config/valheimtooler_settings.cfg for your configured key.
The configuration is created on first launch. Personal configuration files
are not included in this package.

## Changes

- Fixed Reset Terrain's parameter-count exception by supplying the required
  paintOnly=false argument to TerrainComp.Save.
- Reset now claims and verifies terrain ownership before changing terrain arrays,
  so an unsuccessful ownership request cannot leave an unsaved local reset.
- Updated item-giving, player-message, status-effect and terrain-refresh calls
  for the targeted game version.
- Restored the matching unlimited-stamina patch target.
- Added item-loading and terrain null checks.
- Adjusted input patches and terrain execution.
- Custom terrain settings are applied through the native terrain operation
  on locally owned terrain, retaining its save and refresh logic.

## Known Limitations

This package has passed static DLL and API checks, but full feature and
multiplayer compatibility have not been verified. Do not treat it as a fully
tested Valheim 1.0 release.

- Server Devcommands can override fly, god and ghost states. Its permissions
  and devcommands configuration still apply; these modes have unresolved
  reports in the tested mod combination.
- Terrain operations depend on terrain ownership and retain the game's
  normal height limits. Terrain behavior still needs in-game verification.
- The separate terrain-height-limit removal plugin is not included.
- Other mods, game updates and server rules may affect behavior.

## Credits and License

Fork publisher and maintainer: EggyTeam.
Original ValheimTooler author: Yohann MARTIN (Astropilot).
Base distribution: CookiexMilk-ValheimTooler 1.11.2.
Upstream source: https://github.com/Astropilot/ValheimTooler

The package website points to EggyTeam's fork listing. The source link above is upstream.
The original MIT license and copyright notice are included in LICENSE.
SharpConfig.dll is retained as a runtime dependency from the base distribution.
