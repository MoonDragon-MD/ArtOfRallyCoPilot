using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using ArtOfRallyCoPilot;

namespace ArtOfRallyCoPilot.Loader
{
    public static class AudioAssetLoader
    {
        private const string CoPilotAssetPath = "CoPilotAssets";

        public static Dictionary<string, AudioClip> LoadAudioAssets()
        {
            var directoryPath = Path.Combine(Main.ModEntry.Path, CoPilotAssetPath, Main.Settings.AssetSet);
            Main.Logger.Log($"Tentativo di caricare audio da: {directoryPath}");
            var directory = new DirectoryInfo(directoryPath);
            var audioClips = new Dictionary<string, AudioClip>();

            if (!directory.Exists)
            {
                Main.Logger.Log("[WARNING] Directory audio non trovata: " + directoryPath);
                return audioClips;
            }

            foreach (var file in directory.GetFiles("*.wav"))
            {
                try
                {
                    Main.Logger.Log($"Caricamento file WAV: {file.FullName}");
                    var audioClip = LoadWAV(file.FullName);
                    if (audioClip != null)
                    {
                        audioClips[Path.GetFileNameWithoutExtension(file.Name)] = audioClip;
                        Main.Logger.Log($"Audio clip caricato: {Path.GetFileNameWithoutExtension(file.Name)}");
                    }
                    else
                    {
                        Main.Logger.Log("[WARNING] Impossibile caricare il clip: " + file.Name);
                    }
                }
                catch (Exception e)
                {
                    Main.Logger.Log($"[ERROR] Errore caricamento WAV {file.Name}: {e.Message}\nStackTrace: {e.StackTrace}");
                }
            }

            Main.Logger.Log($"Totale audio clips caricati: {audioClips.Count}");
            return audioClips;
        }

        private static AudioClip LoadWAV(string path)
        {
            try
            {
                Main.Logger.Log($"Lettura dati da: {path}");
                byte[] fileData = File.ReadAllBytes(path);
                Main.Logger.Log($"Dati letti, lunghezza: {fileData.Length} byte");

                // Usa WavUtility.ToAudioClip con il nome del file
                AudioClip audioClip = WavUtility.ToAudioClip(fileData, 0, Path.GetFileNameWithoutExtension(path));
                if (audioClip != null)
                {
                    Main.Logger.Log($"AudioClip creato con successo per: {path} (Samples: {audioClip.samples}, Channels: {audioClip.channels}, Frequency: {audioClip.frequency})");
                    return audioClip;
                }
                else
                {
                    Main.Logger.Log("[WARNING] WavUtility non ha creato un AudioClip valido per: " + path);
                    return null;
                }
            }
            catch (Exception e)
            {
                Main.Logger.Log($"[ERROR] Errore durante il parsing di {path}: {e.Message}\nStackTrace: {e.StackTrace}");
                return null;
            }
        }
    }
}