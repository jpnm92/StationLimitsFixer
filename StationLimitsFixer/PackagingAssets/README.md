# Station Limits Fixer (FAFO Edition)

**Disclaimer:** This is my first mod. It is the direct result of me "fucking around and finding out" because Valheim's building restrictions are... bullshit!

## TL;DR
Sick of Valheim's placement restrictions? This mod shrinks crafting station extension hitboxes so they take up less space, extends their connection range to 25+ meters, lets heavy stations (smelters, kilns, blast furnaces, windmills) ignore uneven terrain, and makes roof requirements optional. Install it, tweak the config if needed. Respects Wards so you can't use it to grief.

## Installation
1. **Via Gale (Recommended):** Search for "Station Limits Fixer" in [Gale](https://github.com/MythicManiac/Gale) and install.
2. **Manual:** Extract the DLL file to your `BepInEx/plugins` folder.
3. **Via Mod Manager:** Most Valheim mod managers support Thunderstore, where this mod is hosted.
4. **Start the game.** Default config values apply automatically on first launch.

## Why did I make this?
Because I got tired of this dumbass game telling me there's no space when very obviously I HAVE THE FUCKING SPACE.

## Features

### Hitbox Exclusions (Smart Filter)
The mod makes workbench extensions physically tiny so they don't take up space and can be placed close together. This is done by dynamically shrinking the collision boxes of all extension pieces.

- **Smart Filtering:** The mod intelligently ignores trigger colliders, so you can still interact with stations and press 'E' to use them.
- **Configurable Hitbox Size:** Adjust `ImprovementHitboxSize` to control the physical footprint (default: 0.6). Lower values = smaller hitboxes.
- **Escape Hatch:** If a piece breaks or looks visually weird after shrinking, add its prefab name to the `ExcludedHitboxPieces` config and the mod will skip it. By default, the Black Forge Cooler and Tanning Rack are excluded for visual fidelity.

### Smelter Override Behavior (FAFO)
Tired of the game refusing to place Smelters, Kilns, Blast Furnaces, and Windmills because the terrain isn't perfectly flat?

- **Brute Force Placement:** Enabled by default. Overrides physics checks so these specific heavy crafting stations can be placed anywhere.
- **Wood Floor Safety:** If `AllowSmeltersOnWood` is disabled (default), the mod will still prevent you from placing smelters on wooden floors, respecting the vanilla balance.
- **Ward Respect:** The brute force system respects Valheim's Ward/territory system, so you can't use it to place crafting stations in other players' protected areas (grief-proofing).

### Extended Connection Range
Extensions now connect from 25+ meters away by default, instead of the vanilla ~10 meters.

- **Increased Distance:** Adjust `MaxRange` to change the connection distance (default: 25). Put your upgrades in the basement, attic, or garden—anywhere within range.
- **AzuWorkbenchTweaks Compatibility:** If you have AzuWorkbenchTweaks installed, this mod automatically yields control of connection range to avoid conflicts.

### Optional Roof Requirements
Toggle whether your main crafting stations (workbenches, smelters, etc.) require a roof.

- **NoRoofRequired Config:** Set to `true` to let stations work in the rain or under the open sky (default: `false`). Perfect for outdoor or underground setups without massive roof structures.
- **AzuWorkbenchTweaks Compatibility:** If you have AzuWorkbenchTweaks installed, this setting is disabled to prevent mod conflicts.

### Universal Mod Compatibility
Valheim's modding ecosystem is growing. This mod doesn't rely on a hardcoded list of pieces—it scans **every loaded object** in the game and applies the same fixes to all custom crafting stations and extensions added by other mods.

- **Dynamic Scanning:** New magic forges, custom workbenches, or themed crafting stations from other mods? They're automatically patched without mod updates.
- **Auto Recipe Scanning:** Optionally forces a refresh of your known recipes list every time you interact with a crafting station (enabled by default). Fixes recipe injection delays from heavily modded item pools.

### Configuration
Most settings are available in the BepInEx config file (`BepInEx/config/com.custom.stationlimits.cfg`). Common tweaks:

| Setting | Description | Default |
|---------|-------------|---------|
| `ImprovementHitboxSize` | Multiplier for extension hitbox size (lower = smaller) | 0.6 |
| `MaxRange` | Maximum distance extensions connect to stations (meters) | 25 |
| `NoRoofRequired` | Toggle roof requirement for crafting stations | false |
| `ForceSmelterPlacement` | Brute force smelter placement on uneven terrain | true |
| `AllowSmeltersOnWood` | Allow heavy smelters on wooden floors | false |
| `ExcludedHitboxPieces` | Comma-separated prefab names to skip hitbox shrinking | blackforge_ext1,piece_workbench_ext2 |
| `AutoScanRecipes` | Auto-scan recipes when opening a station | true |

## Multiplayer & Safety
- **Ward System:** Fully respects Valheim's Ward system. You cannot use this mod to bypass protection and place structures in other players' bases.
- **Seamless Compatibility:** Soft-depends on AzuWorkbenchTweaks to avoid conflicts if you use both mods.

## Why am I so rude?
I am honestly pissed off I couldn't find any mod that does what I just did. Also I kind of enjoy being an ass. It is what it is.

---
**Source & Issues:** [https://github.com/jpnm92/StationLimitsFixer](https://github.com/jpnm92/StationLimitsFixer)

## Check out my other mod
**InventoryYeeter**: Instantly yeet your entire inventory with a single keypress. Panic button (Default: G) to jettison your heaviest non-equipped items until you are no longer encumbered. Perfect for rage-quitting or just cleaning up fast.

- [InventoryYeeter on Thunderstore](https://thunderstore.io/c/valheim/p/jpnm92/InventoryYeeter/)
- [InventoryYeeter on NexusMods](https://www.nexusmods.com/valheim/mods/2592)