using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;
using System.Collections.Generic;

namespace StationLimitsFixer
{
    [BepInPlugin("com.custom.stationlimits", "Station Limits Fixer", "1.0.0")]
    public class StationFixerPlugin : BaseUnityPlugin
    {
        public static ConfigEntry<float> HitboxSize;
        public static ConfigEntry<float> MaxConnectionDistance;
        public static ConfigEntry<bool> RemoveRoofRequirement;

        void Awake()
        {
            HitboxSize = Config.Bind("General", "ImprovementHitboxSize", 0.6f, "Size of the physical footprint. 0.1 is tiny.");
            MaxConnectionDistance = Config.Bind("General", "MaxRange", 25f, "Maximum distance between a crafting station and its improvements.");
            RemoveRoofRequirement = Config.Bind("General", "NoRoofRequired", false, "If true, main crafting stations work even without a roof or in the rain.");

            HitboxSize.SettingChanged += (sender, args) => ApplyChanges();
            MaxConnectionDistance.SettingChanged += (sender, args) => ApplyChanges();
            RemoveRoofRequirement.SettingChanged += (sender, args) => ApplyChanges();

            Harmony harmony = new Harmony("com.custom.stationlimits");
            harmony.PatchAll();

            Logger.LogInfo("Station Limits Fixer: Classic Targeted Mode Active!");
        }

        [HarmonyPatch(typeof(ZNetScene), "Awake")]
        public static class ZNetScene_Patch
        {
            private static void Postfix() => ApplyChanges();
        }

        public static void ApplyChanges()
        {
            if (ZNetScene.instance == null) return;

            // The exact list, finally including the Chopping Block (ext1)!
            List<string> allPieces = new List<string> {
                "piece_workbench", "forge", "piece_stonecutter", "piece_artisan", "blackforge", "galdrtable", "piece_oven",
                "piece_workbench_ext1", "piece_workbench_ext2", "piece_workbench_ext3", "piece_workbench_ext4",
                "forge_ext1", "forge_ext2", "forge_ext3", "forge_ext4", "forge_ext5", "forge_ext6",
                "piece_cauldron_ext1", "piece_cauldron_ext2", "piece_cauldron_ext3", "piece_cauldron_ext4",
                "blackforge_ext1", "blackforge_ext2", "galdrtable_ext1", "galdrtable_ext2"
            };

            foreach (string name in allPieces)
            {
                // We go straight to the source, no messy loops through the whole game
                GameObject prefab = ZNetScene.instance.GetPrefab(name);
                if (prefab == null) continue;

                // 1. Kill the "Needs more space" building restriction
                Piece piece = prefab.GetComponent<Piece>();
                if (piece != null)
                {
                    piece.m_spaceRequirement = 0f;
                }

                // 2. Shrink all colliders to absolute dust
                Collider[] colliders = prefab.GetComponentsInChildren<Collider>();
                foreach (Collider col in colliders)
                {
                    float s = HitboxSize.Value;
                    if (col is BoxCollider box) box.size = new Vector3(s, s, s);
                    else if (col is CapsuleCollider capsule) { capsule.radius = s / 2f; capsule.height = s; }
                    else if (col is SphereCollider sphere) sphere.radius = s / 2f;
                }

                // 3. Station Logic Fixes
                StationExtension ext = prefab.GetComponent<StationExtension>();
                if (ext != null)
                {
                    Traverse.Create(ext).Field("m_maxStationDistance").SetValue(MaxConnectionDistance.Value);
                    Traverse.Create(ext).Field("m_continousConnection").SetValue(true);
                }

                CraftingStation station = prefab.GetComponent<CraftingStation>();
                if (station != null)
                {
                    station.m_craftRequireRoof = !RemoveRoofRequirement.Value;
                }
            }
        }
    }
}