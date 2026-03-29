# Changelog
## [1.4.1] - Forgot to update the description hehe.
## [1.4.0] - The Performance & Polish Update
- **Zero-GC Performance Overhaul:** Completely replaced slow `MethodInfo.Invoke` reflection calls with ultra-fast Harmony Delegates for both brute-force placement and the new auto-recipe scanning. This eliminates micro-stutters and garbage collection overhead.
- **Seamless Mod Compatibility:** Added BepInEx SoftDependency support for `Azumatt.AzuWorkbenchTweaks`. If detected, this mod automatically hides its UI sliders and yields control of workbench range and roof requirements to prevent mod conflicts.
- **Config Slider Debouncing:** Added a coroutine delay (0.5s) to the BepInEx Configuration Manager. Dragging the hitbox slider back and forth no longer freezes the game with endless hierarchy scans.
- **Hitbox Scaling Bug Fix:** Hitboxes now cache their original default sizes into dictionaries upon first load. Tweaking the scale config mid-game no longer causes a runaway "compounding shrink" loop.
- **Smarter Wood Floor Checks:** The `AllowSmeltersOnWood` logic now uses a downward physics raycast instead of the unreliable vanilla `RequireGround` status. You can now properly place smelters on overhanging natural rock ledges without the mod blocking it.
- **Auto Recipe Scan Fix:** Added `AutoScanRecipes` config and a new Harmony patch. If enabled, it automatically forces the game to update your known recipes list when interacting with a station, fixing injection delay bugs from heavily modded item pools.
- **World Load Optimization:** The mod now skips running expensive `FindObjectsByType` active scene searches during initial world generation when no objects exist yet.

## [1.3.0] - The Optimization & Compatibility Update
- **Universal Mod Compatibility:** Removed the hardcoded list of vanilla pieces. The mod now dynamically scans all loaded items, automatically applying roof, range, and hitbox fixes to **all custom crafting stations** added by other mods.
- **Massive Performance Boost:** I was being stupid before and running the hitbox shrinker every single frame. Now it only runs once per piece when it's first loaded, which is way more efficient and should eliminate any potential performance issues.
- **Smart Hitbox Filtering:** The mod now intelligently ignores trigger colliders. This prevents the mod from accidentally shrinking the interaction zones required to actually use the stations.
- **The "Escape Hatch" Config:** Added a new configuration setting (`ExcludedHitboxPieces`) allowing players to manually skip the hitbox shrinker for specific pieces. By default, the Black Forge Cooler (`blackforge_ext1`) and Tanning Rack (`piece_workbench_ext2`) are excluded to preserve their unique visual models and interaction boundaries.
- **Allow Smelters in Wards:** The Brute Force placement override now respects Valheim's Ward system. Players can no longer use the mod to bypass building restrictions inside another player's active Ward. (Probably! I'm too lazy to set it up and don't have friends to test it. I doubt someone within this use case uses this mod but if there are let me know if it doesn't work).
- **Multiplayer Security (Ward Fix):** The Brute Force placement override now respects Valheim's Ward system. Players can no longer use the mod to bypass building restrictions inside another player's active Ward. (Probably! I'm too lazy to set it up and don't have friends to test it. I doubt someone within this use case uses this mod but if there are let me know if it doesn't work).

## v1.2.2
* **Change 2.1:** Piece Workbench Ext2 is now excluded from hitbox shrinking for better visuals/compatibility. Should still be placeable next to the workbench or any of its extensions.

## [1.2.1]
- **Packaging Update:** Added standard Changelog file directly to mod distribution packages for Thunderstore and Nexus compatibility.

## [1.2.0] - FAFO Edition
- **FAFO Override:** Added `BruteForceSmelters` toggle (enabled by default) to let you place Smelters, Kilns, Blast Furnaces, and Windmills wherever you want. Physics shouldn't stop progress.

## [1.1.0] - The Blackforge & Mistlands Update
- **Mistlands Support:** Full support added for the Blackforge and Galdr Table. 
- **Blackforge Fix:** Shrunk the massive physical footprint of Blackforge extensions so you can actually fit them in your base.
- **Connection Range:** Drastically increased the connection distance for all Mistlands-tier improvements.
- **Rain Protection:** Added the "No Roof Required" toggle so your Blackforge and Eitri refinery setups don't need a massive roof in the middle of a build.
- **Expanded List:** Now covers Artisan Tables, Stonecutters, and all Cauldron upgrades.