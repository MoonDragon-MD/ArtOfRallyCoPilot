using System;
using System.Reflection;
using ArtOfRallyCoPilot.Loader;
using ArtOfRallyCoPilot.Settings;
using HarmonyLib;
using UnityModManagerNet;

namespace ArtOfRallyCoPilot
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class Main
    {
        public static CoPilotSettings Settings;

        public static UnityModManager.ModEntry.ModLogger Logger;
        
        public static UnityModManager.ModEntry ModEntry;

        static bool Load(UnityModManager.ModEntry modEntry)
        {
            ModEntry = modEntry;
            Logger = modEntry.Logger;
            var harmony = new Harmony(modEntry.Info.Id);
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            try
            {
                Settings = UnityModManager.ModSettings.Load<CoPilotSettings>(modEntry);
                CoPilot.Textures = CoPilotAssetLoader.LoadAssets();
                modEntry.OnUpdate = (entry, dt) => InitializeAudio();
                modEntry.OnGUI = entry => Settings.Draw(entry);
                modEntry.OnSaveGUI = entry => Settings.Save(entry);
                modEntry.OnFixedGUI = CoPilot.Draw;
            }
            catch (Exception e)
            {
                Logger.Log($"[ERROR] Errore durante l'inizializzazione: {e.Message}");
                return false;
            }

            return true;
        }

        private static bool audioInitialized = false;

        private static void InitializeAudio()
        {
            if (audioInitialized) return;

            try
            {
                Logger.Log("Inizializzazione audio in corso...");
                CoPilot.AudioClips = AudioAssetLoader.LoadAudioAssets();
                if (CoPilot.AudioClips != null && CoPilot.AudioClips.Count > 0)
                {
                    Logger.Log($"Audio clips caricati: {CoPilot.AudioClips.Count}");
                    audioInitialized = true;
                }
                else
                {
                    Logger.Log("[WARNING] Nessun audio clip caricato.");
                }
            }
            catch (Exception e)
            {
                Logger.Log($"[ERROR] Errore durante l'inizializzazione dell'audio: {e.Message}");
            }
        }
    }
}