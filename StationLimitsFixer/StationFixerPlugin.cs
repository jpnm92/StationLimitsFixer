using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;
using System.Collections.Generic;

namespace StationLimitsFixer
{
    [BepInPlugin("com.custom.stationlimits", "Station Limits Fixer", "1.2.2")] // FAFO Edition
    public class StationFixerPlugin : BaseUnityPlugin
    {
        public static ConfigEntry<float> HitboxSize;
        public static ConfigEntry<float> MaxConnectionDistance;
        public static ConfigEntry<bool> RemoveRoofRequirement;

        // NEW CONFIG: The Brute Force Toggle
        public static ConfigEntry<bool> BruteForceSmelters;

        internal static ManualLogSource StaticLogger;

        void Awake()
        {
            StaticLogger = Logger; // Set up the logger for static methods

            HitboxSize = Config.Bind("General", "ImprovementHitboxSize", 0.6f, "Size of the physical footprint. 0.1 is tiny.");
            MaxConnectionDistance = Config.Bind("General", "MaxRange", 25f, "Maximum distance between a crafting station and its improvements.");
            RemoveRoofRequirement = Config.Bind("General", "NoRoofRequired", false, "If true, main crafting stations work even without a roof or in the rain.");

            // NEW CONFIG: Default is true (because we hate physics)
            BruteForceSmelters = Config.Bind("Advanced", "ForceSmelterPlacement", true, "If true, Smelters, Kilns, and Windmills can be placed ANYWHERE, ignoring terrain rules.");

            HitboxSize.SettingChanged += (sender, args) => ApplyChanges();
            MaxConnectionDistance.SettingChanged += (sender, args) => ApplyChanges();
            RemoveRoofRequirement.SettingChanged += (sender, args) => ApplyChanges();

            Harmony harmony = new Harmony("com.custom.stationlimits");
            harmony.PatchAll();

            StaticLogger.LogInfo("Station Limits Fixer: FAFO Edition Initialized!");
        }

        [HarmonyPatch(typeof(ZNetScene), "Awake")]
        public static class ZNetScene_Patch
        {
            private static void Postfix() => ApplyChanges();
        }

        public static void ApplyChanges()
        {
            if (ZNetScene.instance == null) return;

            List<string> allPieces = new List<string> {
                "piece_workbench", "forge", "piece_stonecutter", "piece_artisan", "blackforge", "galdrtable", "piece_oven",
                "piece_workbench_ext1", "piece_workbench_ext2", "piece_workbench_ext3", "piece_workbench_ext4",
                "forge_ext1", "forge_ext2", "forge_ext3", "forge_ext4", "forge_ext5", "forge_ext6",
                "piece_cauldron_ext1", "piece_cauldron_ext2", "piece_cauldron_ext3", "piece_cauldron_ext4",
                "blackforge_ext1", "blackforge_ext2", "blackforge_ext3", "galdrtable_ext1", "galdrtable_ext2"
            };

            int count = 0;
            foreach (string name in allPieces)
            {
                GameObject prefab = ZNetScene.instance.GetPrefab(name);
                if (prefab == null) continue;

                // 1. Kill building restriction
                Piece piece = prefab.GetComponent<Piece>();
                if (piece != null) piece.m_spaceRequirement = 0f;

                // 2. Shrink Hitboxes - EXCEPT the Black Forge Cooler (ext1) to stop the floating issue
                //2.1 - The Piece Workbench Ext2 is also excluded because it has a unique hitbox that doesn't cause issues and looks better when left alone
                if (name != "blackforge_ext1" && name != "piece_workbench_ext2")
                {
                    Collider[] colliders = prefab.GetComponentsInChildren<Collider>();
                    foreach (Collider col in colliders)
                    {
                        float s = HitboxSize.Value;
                        if (col is BoxCollider box) box.size = new Vector3(s, s, s);
                        else if (col is CapsuleCollider capsule) { capsule.radius = s / 2f; capsule.height = s; }
                        else if (col is SphereCollider sphere) sphere.radius = s / 2f;
                    }
                }

                // 3. Extension Logic (Range Fix)
                StationExtension ext = prefab.GetComponent<StationExtension>();
                if (ext != null)
                {
                    Traverse.Create(ext).Field("m_maxStationDistance").SetValue(MaxConnectionDistance.Value);
                    Traverse.Create(ext).Field("m_continousConnection").SetValue(true);
                }

                // 4. Crafting Station Logic (Roof Fix)
                CraftingStation station = prefab.GetComponent<CraftingStation>();
                if (station != null)
                {
                    station.m_craftRequireRoof = !RemoveRoofRequirement.Value;
                }

                count++;
            }
            StaticLogger.LogInfo($"Successfully patched {count} crafting pieces.");
        }

        // ------------------------------------------------------------------
        // NEW FEATURE: The Brute Force Placement Patch
        // ------------------------------------------------------------------
        [HarmonyPatch(typeof(Player), "UpdatePlacementGhost")]
        public static class BruteForcePlacementPatch
        {
            public static void Postfix(Player __instance, ref GameObject ___m_placementGhost)
            {
                // If the user turned it off in the config, skip this entire patch
                if (!BruteForceSmelters.Value) return;

                if (___m_placementGhost == null) return;

                Piece piece = ___m_placementGhost.GetComponent<Piece>();

                string[] bulkyPieces = { 
                    "$piece_smelter", 
                    "$piece_charcoalkiln", 
                    "$piece_blastfurnace", 
                    "$piece_windmill" 
                };

                // If the piece is in our list, override the physics engine
                if (piece != null && System.Array.Exists(bulkyPieces, name => name == piece.m_name))
                {
                    try
                    {
                        // Safely set the m_placementStatus enum
                        var statusField = Traverse.Create(__instance).Field("m_placementStatus");
                        System.Type enumType = statusField.GetValueType();
                        if (enumType != null)
                        {
                            statusField.SetValue(System.Enum.ToObject(enumType, 0)); // 0 = Valid
                        }
                        else
                        {
                            statusField.SetValue(0);
                        }

                        // Safely invoke SetPlacementGhostValid
                        var methodWithBool = Traverse.Create(__instance).Method("SetPlacementGhostValid", new System.Type[] { typeof(bool) });
                        if (methodWithBool.MethodExists())
                        {
                            methodWithBool.GetValue(true);
                        }
                        else
                        {
                            var methodNoArgs = Traverse.Create(__instance).Method("SetPlacementGhostValid");
                            if (methodNoArgs.MethodExists())
                            {
                                methodNoArgs.GetValue();
                            }
                        }

                        // Also try the Piece's SetInvalidPlacementHeightlight (often used to clear red color)
                        var heightlightMethod = Traverse.Create(piece).Method("SetInvalidPlacementHeightlight", new System.Type[] { typeof(bool) });
                        if (heightlightMethod.MethodExists())
                        {
                            heightlightMethod.GetValue(false);
                        }
                    }
                    catch (System.Exception ex)
                    {
                        StaticLogger.LogError($"[Station Limits Fixer] Failed to override placement for {piece.m_name}: {ex.Message}");
                    }
                }
            }
        }
    }
}