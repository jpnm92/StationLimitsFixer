# ⚠️ IMPORTANT FOR 1.4.1 UPDATE
**If you are updating from version 1.2.2, I recommend you to delete the old config file**
It's not necessarily needed but the new default config file represents better default behavior for smelters, kilns, mills.

# Station Limits Fixer (FAFO Edition)

**Disclaimer:** This is my first mod. It is the direct result of me "fucking around and finding out" because Valheim's building restrictions are... bullshit! If it breaks your game, you probably shouldn't have trusted a guy who just learned what a BoxCollider was yesterday. 

*(It's actually highly optimized now and doesn't add any new assets, only edits. So even if you uninstall it mid-playthrough, your world won't explode. You might just have to replace some workstations).*

## Why did I make this?
Because I got tired of this dumbass game telling me there's no space when very obviously I HAVE THE FUCKING SPACE.

## What this actually does:
- **Universal Mod Compatibility:** I originally hardcoded a list of vanilla pieces, but that was stupid. Now, the mod dynamically scans every single loaded object in the game's memory. If you download another mod that adds a "Magic Bullshit Forge", this mod will automatically fix its range and hitboxes without me having to write a patch for it.
- **Surgical Hitbox Shrinking:** The mod makes workbench "improvements" physically tiny so you can stack them like sardines. However, it now features a "Smart Filter" that ignores trigger colliders, meaning you can still actually look at them and press 'E' to interact.
- **The "Escape Hatch" Config:** Sometimes shrinking a hitbox makes a piece look visually broken. Instead of me constantly updating the mod to fix edge cases, I added an `ExcludedHitboxPieces` config. If a piece breaks, just type its prefab name in the config, and the mod will ignore it. *(The Black Forge Cooler and the Tanning Rack are excluded by default).*
- **Brute Force Smelters (FAFO Override):** Enabled by default. Tired of the game telling you a Blast Furnace or Windmill needs perfectly flat dirt? This overrides the physics engine so you can place them anywhere. **Bonus:** It actually respects the game's Ward system, so you can't use this to bypass restrictions and drop a Smelter in the middle of your friend's living room.
- **Brain-Dead Range:** Extensions now connect from 25+ meters away. Put your upgrades in the basement, the attic, the garden or your mother—you know, as long as she's between the set distance in the config, the workbench doesn't care.
- **Roof Needed (Optional):** You can toggle whether your main crafting stations need a roof or not. The default is vanilla behavior (because sometimes you want to stay a little bit of a Viking).

## Installation:
1. Extract the DLL to your `BepInEx/plugins` folder or just install with r2modman like a normal person.

## Why am I so rude?
I am honestly pissed off I couldn't find any mod that does what I just did. Also I kind of enjoy being an ass. It is what it is.

---
**Source & Issues:** [https://github.com/jpnm92/StationLimitsFixer](https://github.com/jpnm92/StationLimitsFixer)

## Check out my other mod
**InventoryYeeter**: Instantly yeet your entire inventory with a single keypress. Panic button (Default: G) to jettison your heaviest non-equipped items until you are no longer encumbered. Perfect for rage-quitting or just cleaning up fast.

- [InventoryYeeter on Thunderstore](https://thunderstore.io/c/valheim/p/jpnm92/InventoryYeeter/)
- [InventoryYeeter on NexusMods](https://www.nexusmods.com/valheim/mods/2592)