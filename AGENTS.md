# StationLimitsFixer — Agent Guide

## Overview
Valheim BepInEx mod (C# / .NET Framework 4.7.2) that:
- Shrinks physical hitboxes of crafting station improvements (StationExtension pieces)
- Extends max connection distance between stations and their improvements
- Optionally removes roof requirement from crafting stations
- Brute-forces smelter/kiln placement on uneven terrain
- Auto-scans recipes when opening a crafting station

## Project Structure
```
StationLimitsFixer/
├── StationLimitsFixer.slnx          # Solution file
├── StationLimitsFixer/
│   ├── StationFixerPlugin.cs        # Main plugin (single-file mod)
│   ├── StationLimitsFixer.csproj    # .NET Framework 4.7.2 project
│   ├── Properties/AssemblyInfo.cs
│   └── PackagingAssets/
│       ├── manifest.json            # Thunderstore manifest
│       ├── README.md / README.txt   # Store descriptions
│       ├── CHANGELOG.md
│       └── icon.png
└── AGENTS.md                        # This file
```

## Build
- Target: .NET Framework 4.7.2, OutputType: Library
- References are local paths to r2modman BepInEx core DLLs and Valheim managed assemblies
- Build command: `msbuild StationLimitsFixer.slnx /p:Configuration=Release`
- Release build auto-zips Thunderstore and Nexus packages into `Releases/`

## Key Dependencies (referenced as local DLLs)
- BepInEx 5.x (core + Harmony)
- Valheim's `assembly_valheim.dll` (game assemblies)
- UnityEngine DLLs from Valheim's Managed folder

## Harmony Patches
1. `ZNetScene.Awake` Postfix — applies config changes to all prefabs on scene load
2. `Player.UpdatePlacementGhost` Postfix — brute-force smelter placement
3. `CraftingStation.Interact` Prefix — force recipe scan

## Important Valheim API Notes
- `StationExtension.m_continousConnection` — when true, connection lines are ALWAYS visible. Do NOT set this to true unless you want permanent line visibility.
- `StationExtension.m_maxStationDistance` — controls max range for connection check. Safe to modify.
- `CraftingStation.m_craftRequireRoof` — only set to false when disabling roof requirement. Never force to true on stations that may lack `m_roofCheckPoint` (causes NRE in `CheckUsable`).
- `CraftingStation.m_roofCheckPoint` — may be null on stations that don't require a roof. Always null-check before relying on roof logic.

## Soft Dependency
- `Azumatt.AzuWorkbenchTweaks` — if present, skip MaxConnectionDistance and RemoveRoofRequirement to avoid conflicts.

## Config Entries
| Section  | Key                    | Type   | Default | Purpose |
|----------|------------------------|--------|---------|---------|
| General  | ImprovementHitboxSize  | float  | 0.6     | Scale factor for extension hitboxes |
| General  | MaxRange               | float  | 25      | Max station-to-extension distance |
| General  | NoRoofRequired         | bool   | false   | Disable roof requirement |
| General  | AutoScanRecipes        | bool   | true    | Scan recipes on station interact |
| Advanced | ForceSmelterPlacement  | bool   | true    | Allow smelters on uneven terrain |
| Advanced | AllowSmeltersOnWood    | bool   | false   | Allow smelters on wooden floors |
| Advanced | ExcludedHitboxPieces   | string | ...     | Comma-separated prefab names to skip |

## Conventions
- Single-file plugin architecture — all logic in `StationFixerPlugin.cs`
- Version in `[BepInPlugin]` attribute must match `manifest.json` version_number
- PostBuildEvent copies DLL to r2modman profile (path is author-specific)
- No unit tests — mod is tested in-game via Valheim + BepInEx

## Git Workflow
- Upstream: `jpnm92/StationLimitsFixer` (master branch)
- Fork: `GrandpaUA/StationLimitsFixer`
- Feature branches from master, PR to upstream
