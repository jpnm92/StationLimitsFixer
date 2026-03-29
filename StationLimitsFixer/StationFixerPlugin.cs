using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;
using System.Collections.Generic;

namespace StationLimitsFixer
{
    [BepInPlugin("com.custom.stationlimits", "Station Limits Fixer", "1.4.1")]
    [BepInDependency("Azumatt.AzuWorkbenchTweaks", BepInDependency.DependencyFlags.SoftDependency)]
    public class StationFixerPlugin : BaseUnityPlugin
    {
        public static ConfigEntry<float> HitboxSize;
        public static ConfigEntry<float> MaxConnectionDistance;
        public static ConfigEntry<bool> RemoveRoofRequirement;
        public static ConfigEntry<bool> BruteForceSmelters;
        public static ConfigEntry<string> ExcludedHitboxPieces;
        public static ConfigEntry<bool> AllowSmeltersOnWood;
        public static ConfigEntry<bool> AutoScanRecipes;

        internal static ManualLogSource StaticLogger;

        private Coroutine _applyChangesCoroutine;

        private static Dictionary<Collider, Vector3> originalBoxSizes = new Dictionary<Collider, Vector3>();
        private static Dictionary<Collider, Vector2> originalCapsuleSizes = new Dictionary<Collider, Vector2>();
        private static Dictionary<Collider, float> originalSphereRadii = new Dictionary<Collider, float>();

        private static bool _azuWorkbenchPresent;

        void Awake()
        {
            // Must be first — everything below may use it
            StaticLogger = Logger;

            _azuWorkbenchPresent = BepInEx.Bootstrap.Chainloader.PluginInfos
                .ContainsKey("Azumatt.AzuWorkbenchTweaks");

            if (_azuWorkbenchPresent)
                StaticLogger.LogWarning("AzuWorkbenchTweaks detected. Skipping MaxConnectionDistance and RemoveRoofRequirement to avoid conflicts.");

            // Always registered
            HitboxSize = Config.Bind("General", "ImprovementHitboxSize", 0.6f, "Size of the physical footprint. 0.1 is tiny.");
            AutoScanRecipes = Config.Bind("General", "AutoScanRecipes", true, "If true, automatically scans for newly unlocked recipes every time you open a crafting station.");
            BruteForceSmelters = Config.Bind("Advanced", "ForceSmelterPlacement", true, "If true, Smelters, Kilns, etc. can be placed on uneven terrain.");
            AllowSmeltersOnWood = Config.Bind("Advanced", "AllowSmeltersOnWood", false, "If true, allows placing heavy Smelters on wooden floors.");
            ExcludedHitboxPieces = Config.Bind("Advanced", "ExcludedHitboxPieces", "blackforge_ext1,piece_workbench_ext2", "Comma-separated list of prefab names to skip when shrinking hitboxes. Applies on load.");

            HitboxSize.SettingChanged += (sender, args) => RequestApplyChanges();

            // Only registered if AzuWorkbenchTweaks is not present
            if (!_azuWorkbenchPresent)
            {
                MaxConnectionDistance = Config.Bind("General", "MaxRange", 25f, "Maximum distance between a crafting station and its improvements.");
                RemoveRoofRequirement = Config.Bind("General", "NoRoofRequired", false, "If true, main crafting stations work even without a roof or in the rain.");

                MaxConnectionDistance.SettingChanged += (sender, args) => RequestApplyChanges();
                RemoveRoofRequirement.SettingChanged += (sender, args) => RequestApplyChanges();
            }

            Harmony harmony = new Harmony("com.custom.stationlimits");
            harmony.PatchAll();

            StaticLogger.LogInfo("Station Limits Fixer initialized.");
        }

        private void RequestApplyChanges()
        {
            if (_applyChangesCoroutine != null) StopCoroutine(_applyChangesCoroutine);
            _applyChangesCoroutine = StartCoroutine(DebouncedApplyChanges());
        }

        private System.Collections.IEnumerator DebouncedApplyChanges()
        {
            yield return new WaitForSeconds(0.5f);
            ApplyChanges(true);
            _applyChangesCoroutine = null;
        }

        [HarmonyPatch(typeof(ZNetScene), "Awake")]
        public static class ZNetScene_Patch
        {
            private static void Postfix() => ApplyChanges(false);
        }

        public static void ApplyChanges(bool updateActiveSceneObjects)
        {
            if (ZNetScene.instance == null) return;

            int count = 0;
            HashSet<string> excludedSet = new HashSet<string>(ExcludedHitboxPieces.Value.Replace(" ", "").Split(','));

            foreach (GameObject prefab in ZNetScene.instance.m_prefabs)
            {
                prefab.TryGetComponent<CraftingStation>(out var station);
                prefab.TryGetComponent<StationExtension>(out var ext);

                if (station == null && ext == null) continue;

                if (prefab.TryGetComponent<Piece>(out var piece))
                    piece.m_spaceRequirement = 0f;

                if (ext != null && !excludedSet.Contains(prefab.name))
                {
                    Collider[] colliders = prefab.GetComponentsInChildren<Collider>();
                    foreach (Collider col in colliders)
                    {
                        if (col.isTrigger) continue;

                        float scale = HitboxSize.Value;

                        if (col is BoxCollider box)
                        {
                            if (!originalBoxSizes.ContainsKey(box)) originalBoxSizes[box] = box.size;
                            box.size = originalBoxSizes[box] * scale;
                        }
                        else if (col is CapsuleCollider capsule)
                        {
                            if (!originalCapsuleSizes.ContainsKey(capsule)) originalCapsuleSizes[capsule] = new Vector2(capsule.radius, capsule.height);
                            capsule.radius = originalCapsuleSizes[capsule].x * scale;
                            capsule.height = originalCapsuleSizes[capsule].y * scale;
                        }
                        else if (col is SphereCollider sphere)
                        {
                            if (!originalSphereRadii.ContainsKey(sphere)) originalSphereRadii[sphere] = sphere.radius;
                            sphere.radius = originalSphereRadii[sphere] * scale;
                        }
                    }
                }

                // Only touch these fields if AzuWorkbenchTweaks isn't managing them
                if (ext != null)
                {
                    if (!_azuWorkbenchPresent)
                        ext.m_maxStationDistance = MaxConnectionDistance.Value;
                    ext.m_continousConnection = true;
                }

                if (station != null && !_azuWorkbenchPresent)
                    station.m_craftRequireRoof = !RemoveRoofRequirement.Value;

                count++;
            }

            if (updateActiveSceneObjects)
            {
                StationExtension[] activeExtensions = UnityEngine.Object.FindObjectsByType<StationExtension>(FindObjectsSortMode.None);
                foreach (StationExtension activeExt in activeExtensions)
                {
                    if (!_azuWorkbenchPresent)
                        activeExt.m_maxStationDistance = MaxConnectionDistance.Value;
                    activeExt.m_continousConnection = true;
                }

                if (!_azuWorkbenchPresent)
                {
                    CraftingStation[] activeStations = UnityEngine.Object.FindObjectsByType<CraftingStation>(FindObjectsSortMode.None);
                    foreach (CraftingStation activeStat in activeStations)
                        activeStat.m_craftRequireRoof = !RemoveRoofRequirement.Value;
                }
            }

            StaticLogger.LogInfo($"Successfully patched {count} prefabs.");
        }

        // --- BruteForcePlacementPatch and ForceRecipeScanPatch unchanged below ---

        [HarmonyPatch(typeof(Player), "UpdatePlacementGhost")]
        public static class BruteForcePlacementPatch
        {
            private delegate void SetPlacementGhostValid_Bool_Delegate(Player instance, bool isValid);
            private delegate void SetPlacementGhostValid_Void_Delegate(Player instance);
            private delegate void SetInvalidPlacementHighlight_Delegate(Piece instance, bool invalid);

            private static SetPlacementGhostValid_Bool_Delegate SetGhostValidBool;
            private static SetPlacementGhostValid_Void_Delegate SetGhostValidVoid;
            private static SetInvalidPlacementHighlight_Delegate SetHighlight;

            private static readonly HashSet<string> bulkyPieces = new HashSet<string>
            { "$piece_smelter", "$piece_charcoalkiln", "$piece_blastfurnace", "$piece_windmill" };

            [HarmonyPrepare]
            static void Prepare()
            {
                var methodValidBool = AccessTools.Method(typeof(Player), "SetPlacementGhostValid", new System.Type[] { typeof(bool) });
                if (methodValidBool != null) SetGhostValidBool = AccessTools.MethodDelegate<SetPlacementGhostValid_Bool_Delegate>(methodValidBool);
                else
                {
                    var methodValidVoid = AccessTools.Method(typeof(Player), "SetPlacementGhostValid", new System.Type[0]);
                    if (methodValidVoid != null) SetGhostValidVoid = AccessTools.MethodDelegate<SetPlacementGhostValid_Void_Delegate>(methodValidVoid);
                }

                var methodHighlight = AccessTools.Method(typeof(Piece), "SetInvalidPlacementHeightlight", new System.Type[] { typeof(bool) });
                if (methodHighlight != null) SetHighlight = AccessTools.MethodDelegate<SetInvalidPlacementHighlight_Delegate>(methodHighlight);
            }

            public static void Postfix(Player __instance, ref GameObject ___m_placementGhost, ref Player.PlacementStatus ___m_placementStatus)
            {
                if (!BruteForceSmelters.Value || ___m_placementGhost == null) return;

                Piece piece = ___m_placementGhost.GetComponent<Piece>();

                if (piece != null && bulkyPieces.Contains(piece.m_name))
                {
                    if (!PrivateArea.CheckAccess(___m_placementGhost.transform.position)) return;

                    if (!AllowSmeltersOnWood.Value && piece.m_groundOnly)
                    {
                        if (Physics.Raycast(___m_placementGhost.transform.position + Vector3.up, Vector3.down, out RaycastHit hit, 2f, LayerMask.GetMask("piece", "Default", "static_solid")))
                        {
                            if (hit.collider.GetComponentInParent<Piece>() != null) return;
                        }
                    }

                    try
                    {
                        ___m_placementStatus = Player.PlacementStatus.Valid;
                        if (SetGhostValidBool != null) SetGhostValidBool(__instance, true);
                        else if (SetGhostValidVoid != null) SetGhostValidVoid(__instance);
                        if (SetHighlight != null) SetHighlight(piece, false);
                    }
                    catch (System.Exception ex)
                    {
                        StaticLogger.LogError($"[Station Limits Fixer] Failed to override placement for {piece.m_name}: {ex.Message}");
                    }
                }
            }
        }

        [HarmonyPatch(typeof(CraftingStation), "Interact")]
        public static class ForceRecipeScanPatch
        {
            private delegate void UpdateKnownRecipes_Delegate(Player instance);
            private static UpdateKnownRecipes_Delegate UpdateRecipes;

            [HarmonyPrepare]
            static void Prepare()
            {
                var method = AccessTools.Method(typeof(Player), "UpdateKnownRecipesList");
                if (method != null) UpdateRecipes = AccessTools.MethodDelegate<UpdateKnownRecipes_Delegate>(method);
            }

            public static void Prefix(Humanoid user)
            {
                if (!AutoScanRecipes.Value) return;

                if (user != null && user == Player.m_localPlayer && UpdateRecipes != null)
                {
                    try
                    {
                        UpdateRecipes(Player.m_localPlayer);
                    }
                    catch (System.Exception ex)
                    {
                        StaticLogger.LogError($"[Station Limits Fixer] Failed to force recipe scan: {ex.Message}");
                    }
                }
            }
        }
    }
}