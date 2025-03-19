using JetBrains.Annotations;
using UnityEngine;

namespace ArtOfRallyCoPilots
{
    public static class CoPilotManager
    {
        [CanBeNull] 
        public static Vector3[] Waypoints;

        [CanBeNull] 
        public static float[] Elevations;
        
        [CanBeNull] 
        public static float[] MeanElevations;

        [CanBeNull] 
        public static float[] Angles;

        [CanBeNull] 
        public static float[] MeanAngles;

        [CanBeNull] 
        public static float[] Distances;
        
        public static int CurrentWaypointIndex;

        // Aggiunta della proprietà CurrentWaypoint
        public static Vector3 CurrentWaypoint;
    }
}