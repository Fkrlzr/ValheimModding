### NOTE
> This is an updated version of ValheimTooler released under the MIT license. All features and the project structure have been kept original and untouched. This release was made at the request of community members.

![Valheim Tooler Logo](https://www.codexus.fr/valheimtooler/valheimtooler_logo_small.png)

![](https://img.shields.io/github/v/tag/Astropilot/ValheimTooler)
![](https://img.shields.io/github/license/Astropilot/ValheimTooler)
![](https://img.shields.io/badge/Made%20with-%E2%9D%A4%EF%B8%8F-red.svg)

Discord (Support & Track): [https://discord.gg/y2wbG5uQ7y](https://discord.gg/y2wbG5uQ7y)

[About](#about) •
[Install / Usage](#install--usage) •
[Source / Contributing](#source--contributing) •
[Changelog](#changelog)

## About

This mod is a port of the [ValheimTooler](https://github.com/Astropilot/ValheimTooler) on BepInEx.
ValheimTooler is a free and open-source software that allows you to cheat on the Valheim game via a multitude of options offered. Be reasonable some features are particularly game breaker.

Here is the list of features offered by this mod:

* **Player**
  * God Mode, you don't lose any more life
  * Unlimited stamina for you
  * Unlimited stamina for the other players
  * No stamina for the other players
  * Fly Mode, sweet creative mode 🕊️
  * Ghost Mode, the monsters cannot see you
  * No placement cost
  * Explore all your minimap (Irreversible on the world on which it is activated!)
  * Reset your minimap
  * Teleport a player to another player (works only if the player is visible on the minimap), to minimap marker or coordinates
  * Allow teleporting with restricted items
  * Instantly heals a player
  * Instantly heals all players
  * Activate a Guardian Power for you
  * Activate a Guardian Power for all players
  * Raise/Decrease a skill to any level
  * Tame all nearby creatures
  * Unlimited weight for inventory
  * Fast crafting
  * Remove your tombstones
  * Change the interaction distance
* **Terrain Shaper**
  * Level terrain
  * Lower terrain
  * Raise terrain
  * Reset modifications
  * Smooth terrain
* **Entities & Items**
  * Spawn any entity
  * Delete all drops on the ground
  * An Item Giver to add any item you want in your inventory
* **Miscellaneous**
  * Inflict damage to a player (ignores the no-pvp mode)
  * Kill all entities except players
  * Kill all players
  * Auto-pin on the minimap the deposits
  * Send a event message to all players (the yellow one on the middle of the screen)
  * Send a chat message as any username
  * A ESP for players, monsters, pickables, deposits and drops with radius setting

**⚠️ Warning ⚠️**: On each feature that allows you to choose a player, the list will only include players who are at a certain distance from you (quite large but not the whole map for all that). This is not a personal choice but a technical limitation of the game. Until I find a way to get around it (if possible) this behavior will remain.

## Install / Usage

### Using mod manager

Just install the mod from the manager and it should be available.

### Manual install

First, download and install the [BepInEx for Valheim](https://valheim.thunderstore.io/package/denikson/BepInExPack_Valheim/) pack. \
Then download ValheimTooler and extract the `BepInEx/` folder in the game path.

To show/hide the cheat window you can press `Del` on your keyboard.

## Configuration

A configuration file is available to modify some of the tool's parameters, this file is created when the tool is first launched.
The file is available:
- If you use the launcher the file should appear in the same place as the `ValheimToolerLauncher.exe`
- If you use the BepInEx version, it should be available in the `BepInEx/config` folder

The name of the configuration file is `valheimtooler_settings.cfg`

This file allows you to modify the following parameters:
- Change the key that displays/hides the tool, defaulting to the Delete key
- Choose to show the tool when it starts, default is active
- Choose the display language of the program, auto by default (depending on the game language)
- Set up keyboard shortcuts for some features

## Source / Contributing

The tool is open-source and is open to contributions, check the [Github repository](https://github.com/Astropilot/ValheimTooler)!