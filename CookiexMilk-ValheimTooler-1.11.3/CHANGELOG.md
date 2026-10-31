### 1.11.3 (14 September 2026)
* Fixed the tool not loading at all: the patch that blocks inventory clicks while the tool is shown now targets the game's renamed `OnRightDown`
* Fixed the `Item Giver` crashing on start when other mods add items without an item component or icons
* Items given with the `Item Giver` are now flagged as cheated items by the game
* Fixed the `Terrain Shaper` for the game's updated terrain refresh
* Release compatibility for the latest version of the game 1.0.7

### 1.11.2 (10 June 2026)
* Fixed CHANGELOG.md

### 1.11.1 (10 June 2026)
* Updated API calls to use current Valheim platform system (Splatform)
* Fixed `UserInfo` references to use `UserId` (PlatformUserID) instead of deprecated `Gamertag` and `NetworkUserId`
* Removed obsolete `PrivilegeManager` references
* Release compatibility for the latest version of the game 0.221.12

### 1.11.0 (31 October 2024)
* Fixed when `Inf/No Stamina (others)` and `Inf Stamina (me)` were both activated the former were not working
* Removed `Far interaction` feature as it was not very compatible with other mods (see https://github.com/Astropilot/ValheimTooler/issues/45)
* The entity list for the `Spawn entity` have now only real entities
* Release compatibility for the latest version of the game 0.219.14

### 1.10.0 (26 May 2024)
* Added new guardian power for the boss Fader
* New shortcuts for the features "Give guardian power to all players" and "Heal all players"
* When the tool is shown, most game interactions are lock
* Fixed ESP to work with custom render scaling
* Added Chinese translation
* Release compatibility for the latest version of the game 0.218.15

### 1.9.2 (20 November 2023)
* Release compatibility for the latest version of the game 0.217.30

### 1.9.1 (5 October 2023)
* Release compatibility for the latest version of the game 0.217.22

### 1.9.0 (28 August 2023)
* Add possibility to define keyboard shortcuts in the config file for some features
* Be able to remove your own tombstones
* Add a "Terrain Shaper" interface
* UI changed to a darker one that match better the game UI
* You are now able to filter the teleport targets
* You can now change the interaction distance
* Release compatibility for the latest version of the game 0.217.14

### 1.8.0 (30 March 2023)
* Fixed Chat feature
* Fixed possible icon size incompatibility with others mods for the item giver
* Release compatibility for the latest version of the game 0.214.300

### 1.7.0 (29 December 2022)

* Fixed configuration file for BepInEx version
* Fixed usage of skill "All" to apply to all skills
* ESP: Performance improved, fixed deposits and added radius setting
* Teleport: Teleport yourself of other player to a player or a minimap pin, or to coordinates
* New features: Allow teleporting with restricted items, autopin of deposits, fast crafting
* Some features improved like infinite stamina, 0 weight inventory

### 1.6.0 (15 December 2022)

* Compatibility for the latest version of the game 0.212.7 (Mistlands)
* A configuration file is now available to tweak some settings
* The positions of the windows are now saved and restored on new instances
* Added new guardian power for Boss Queen
* The Item Giver will now not crash when other mods add improperly configured new items

### 1.5.0 (12 August 2022)

* Release compatibility for the latest version of the game 0.209.10

### 1.4.0 (10 October 2021)

* Translations are now independent from the game internal translation system in order to prevent issues with other mods
* The prefab list is now always up-to-date with any patch

### 1.2.1 (20 May 2021)

* Fixed README for Thunderstore mod page

### 1.2.0 (13 May 2021)

* Release compatibility for the latest version of the game 0.153.2

### 1.1.0 (31 March 2021)

* Added minimap reset
* Added taming of nearby creatures
* Unlimited weight for the inventory!
* More features for the ESP with the display of players, monsters, pickable items, deposits, and drops
* Some UX improvements
* Added a crash report to the launcher
* Compatible with the latest version of the game 0.148.7


### 1.0.0 (5 March 2021)

First release of the tool