using System.Reflection;
using ArtOfRallyCoPilots.Loader;
using ArtOfRallyCoPilots.Settings;
using HarmonyLib;
using UnityModManagerNet;

namespace ArtOfRallyCoPilots
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class Main
    {
        public static CoPilotsSettings Settings;

        public static UnityModManager.ModEntry.ModLogger Logger;
        
        public static UnityModManager.ModEntry ModEntry;

        // ReSharper disable once ArrangeTypeMemberModifiers
        // ReSharper disable once UnusedMember.Local
        static bool Load(UnityModManager.ModEntry modEntry)
        {
            ModEntry = modEntry;
            Logger = modEntry.Logger;
            var harmony = new Harmony(modEntry.Info.Id);
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            Settings = UnityModManager.ModSettings.Load<CoPilotsSettings>(modEntry);
            CoPilot.Textures = CoPilotAssetLoader.LoadAssets();
            modEntry.OnGUI = entry => Settings.Draw(entry);
            modEntry.OnSaveGUI = entry => Settings.Save(entry);
            modEntry.OnFixedGUI = CoPilot.Draw;

            return true;
        }
    }
}
