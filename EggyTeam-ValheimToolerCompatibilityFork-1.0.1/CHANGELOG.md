# Changelog

## 1.0.1

- Fixed Reset Terrain throwing TargetParameterCountException on Valheim 1.0.12:
  the reflected TerrainComp.Save call now supplies paintOnly=false.
- Reset now verifies terrain ownership before modifying and saving a tile.
- Preserved terrain refresh and network update after saving.
- Clarified EggyTeam's fork attribution separately from original author credits.
- Terrain-height-limit removal remains excluded. Runtime multiplayer testing
  is still required; validation covers DLL structure, calls and reset arguments.

## 1.0.0

- Initial ValheimToolerCompatibilityFork package based on ValheimTooler 1.11.2.
- Includes the current compatibility-patched Tooler DLL targeting Valheim 1.0.12.
- Includes only runtime DLLs, package metadata, documentation, icon and license.
- Excludes backups, development scripts, test artifacts, personal settings,
  game assemblies and the removed terrain-height-limit plugin.
- Documents remaining mode, terrain and multiplayer limitations.
