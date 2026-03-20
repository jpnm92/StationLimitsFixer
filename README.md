# Station Limits Fixer (Valheim Mod)

This is a BepInEx plugin for Valheim designed to improve the building experience by tweaking crafting station requirements.

## What this mod does:
- **Hitbox Adjustment:** Shrinks the physical boundaries of station improvements (anvils, etc.) to allow for tighter placement.
- **Range Extension:** Increases the connection distance between upgrades and main stations (Default: 25m).
- **Roof Toggle:** Optional configuration to allow crafting stations to function without a roof.

## Technical Details:
- **Framework:** BepInEx / HarmonyX
- **Target:** Valheim (Unity)
- **Logic:** Uses Harmony prefix/postfix patching to modify `m_maxStationDistance`, `m_spaceRequirement`, and `m_requiresRoof` variables in real-time.
