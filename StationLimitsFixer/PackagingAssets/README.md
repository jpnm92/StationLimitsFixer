# Station Limits Fixer (Valheim Mod)

This is a BepInEx plugin for Valheim designed to improve the building experience by tweaking crafting station requirements.

## What this mod does:
- **Hitbox Adjustment:** Shrinks the physical boundaries of station improvements (anvils, etc.) to allow for tighter placement.
- **Range Extension:** Increases the connection distance between upgrades and main stations (Default: 25m).
- **Roof Toggle:** Optional configuration to allow crafting stations to function without a roof.
- **Brute Force Placement:** Force placement of bulky structures like Smelters, Kilns, and Windmills anywhere, ignoring terrain and spacing rules.

## Technical Details:
- **Framework:** BepInEx / HarmonyX
- **Target:** Valheim (Unity)
- **Logic:** Uses Harmony prefix/postfix patching to modify `m_maxStationDistance`, `m_spaceRequirement`, and `m_requiresRoof` variables in real-time.

## Changelog
### v1.1.0 - The Blackforge & FAFO Edition
* **FAFO Override:** Added BruteForceSmelters toggle (enabled by default) to let you place Smelters, Kilns, Blast Furnaces, and Windmills wherever you want. Physics shouldn't stop progress.
* **Mistlands Support:** Full support added for the Blackforge and Galdr Table. 
* **Blackforge Fix:** Shrunk the massive physical footprint of Blackforge extensions so you can actually fit them in your base.
* **Connection Range:** Drastically increased the connection distance for all Mistlands-tier improvements.
* **Rain Protection:** Added the "No Roof Required" toggle so your Blackforge and Eitri refinery setups don't need a massive roof in the middle of a build.
* **Expanded List:** Now covers Artisan Tables, Stonecutters, and all Cauldron upgrades.