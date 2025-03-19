using ArtOfRallyCoPilots.Config;
using ArtOfRallyCoPilots.Loader;
using HarmonyLib;
using UnityEngine;

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

namespace ArtOfRallyCoPilots.Patches.OutOfBoundsManager
{
    [HarmonyPatch(typeof(global::OutOfBoundsManager), "FixedUpdate")]
    public class OutOfBoundsManagerFixedUpdatePatch
    {
        // ReSharper disable twice InconsistentNaming
        public static void Postfix(int ___CurrentWaypointIndex)
        {
            CoPilotManager.CurrentWaypointIndex = ___CurrentWaypointIndex;
        }
    }

    [HarmonyPatch(typeof(global::OutOfBoundsManager), nameof(global::OutOfBoundsManager.Start))]
    public class OutOfBoundsManagerStartPatch
    {
        public static void Postfix(Vector3[] ____cachedWaypoints)
        {
            var stage = GameModeManager.RallyManager.RallyData.GetCurrentStage();
            var stageKey = $"{AreaManager.GetAreaStringNotLocalized(stage.Area)}_{stage.Name}";

            Main.Logger.Log($"Stage key: {stageKey}, WaypointCount: {____cachedWaypoints.Length}");

            CoPilotManager.Waypoints = ____cachedWaypoints;
            CoPilotManager.Distances = CoPilotGenerator.GetDistances(____cachedWaypoints);
            CoPilotManager.Angles = CoPilotGenerator.GetAngles(____cachedWaypoints);
            CoPilotManager.MeanAngles = CoPilotGenerator.GetMeanValues(
                CoPilotManager.Angles,
                CoPilotManager.Distances,
                Main.Settings.DistanceTolerance
            );
            CoPilotManager.Elevations = CoPilotGenerator.GetElevations(____cachedWaypoints);
            CoPilotManager.MeanElevations = CoPilotGenerator.GetMeanValues(
                CoPilotManager.Elevations,
                CoPilotManager.Distances,
                Main.Settings.DistanceTolerance
            );
            Main.Logger.Log($"Angles: {CoPilotManager.Angles.Length}");

            CoPilot.CoPilotConfig =
                CoPilotConfigLoader.LoadCoPilotConfig(stageKey, ____cachedWaypoints.Length);
            if (CoPilot.CoPilotConfig == null)
            {
                CoPilot.CoPilotConfig = CoPilotGenerator.GenerateCoPilots(
                    CoPilotManager.Waypoints,
                    CoPilotManager.Distances,
                    CoPilotManager.Angles,
                    CoPilotManager.Elevations,
                    DefaultConfig.CoPilotGeneratorConfig
                )[0];
                Main.Logger.Log("Auto-generated CoPilots");
                Main.Logger.Log($"[{string.Join(", ", CoPilot.CoPilotConfig)}]");
            }
        }
    }
}
