using ArtOfRallyCoPilots.Loader;
using ArtOfRallyCoPilots.Patches.OutOfBoundsManager;
using UnityEngine;
using UnityModManagerNet;

namespace ArtOfRallyCoPilots.Settings
{
    public enum CoPilotsMode
    {
        Off,
        Auto,
        ManualOnly,
        GeneratedOnly
    }

    public class CoPilotsSettings : UnityModManager.ModSettings, IDrawable
    {
        [Header("Modes")] [Draw(DrawType.ToggleGroup)]
        public CoPilotsMode CoPilotsMode = CoPilotsMode.Auto;

        [Draw(DrawType.Toggle)] public bool ShowCurrentWaypoint = false;

        [Draw(DrawType.Field)] public string AssetSet = "default";
        
        [Draw(DrawType.Field)] public string ConfigSet = "default";

        [Header("Positioning")] [Draw(DrawType.Slider, Min = 1, Max = 1000)]
        public int CoPilotSize = 200;
        
        [Draw(DrawType.Slider, Min = 0.0f, Max = 1.0f)] public float PositionY = 0.25f;
        [Draw(DrawType.Slider, Min = 0.0f, Max = 1.0f)] public float PositionX = 0.5f;
        
        [Header("Audio")] [Draw(DrawType.Toggle)]
        public bool EnableAudio = true;
    
        [Draw(DrawType.Slider, Min = 0.0f, Max = 1.0f)]
        public float AudioVolume = 1.0f;
    
        [Draw(DrawType.Field)]
        public string AudioSet = "default";

        [Header("Generation")] [Draw(DrawType.Slider, Min = 0.0f, Max = 100.0f)]
        public float DistanceTolerance = 10.0f; // Aggiunta proprietà DistanceTolerance

        public void OnChange()
        {
        }

        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Save(this, modEntry);
            CoPilot.Textures = CoPilotAssetLoader.LoadAssets();
            if (CoPilotManager.Waypoints != null)
            {
                OutOfBoundsManagerStartPatch.Postfix(CoPilotManager.Waypoints);
            }
        }
    }
}