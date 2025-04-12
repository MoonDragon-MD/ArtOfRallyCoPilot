using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityModManagerNet;
using ArtOfRallyCoPilot;

namespace ArtOfRallyCoPilot
{
    public static class CoPilot
    {
        [CanBeNull] public static string[] CoPilotConfig = null;
        [CanBeNull] public static Dictionary<string, Texture2D> Textures = null;
        [CanBeNull] public static Dictionary<string, AudioClip> AudioClips = null;
        private static AudioSource audioSource;
        private static string CoPilotKey;
        private static float LastTimestamp;
        private static float lastLogTime; // Per limitare i log

        public static void Draw(UnityModManager.ModEntry modEntry)
        {
            var currentWaypointIndex = CoPilotManager.CurrentWaypointIndex;
            var coPilotNote = CoPilotConfig?.Length > currentWaypointIndex ? CoPilotConfig[currentWaypointIndex] : null;

            // Log solo se la nota cambia o ogni 1 secondo
            if (coPilotNote != CoPilotKey || Time.time - lastLogTime >= 1.0f)
            {
                Main.Logger.Log($"Waypoint corrente: {currentWaypointIndex}, Nota: {coPilotNote ?? "null"}");
                lastLogTime = Time.time;
            }

            if (Main.Settings.ShowCurrentWaypoint)
            {
                GUI.Label(
                    new Rect(0, 0, 200, 200),
                    $"Waypoint {currentWaypointIndex}\nCoPilot: {coPilotNote ?? "null"}"
                );
            }

            if (coPilotNote == null || Textures?.ContainsKey(coPilotNote) != true)
            {
                if (coPilotNote != CoPilotKey || Time.time - lastLogTime >= 1.0f)
                {
                    Main.Logger.Log($"[WARNING] Texture non trovata per nota: {coPilotNote ?? "null"}");
                }
                return;
            }

            var texture = Textures![coPilotNote];
            if (texture == null)
            {
                if (coPilotNote != CoPilotKey || Time.time - lastLogTime >= 1.0f)
                {
                    Main.Logger.Log($"[ERROR] Texture nulla per nota: {coPilotNote}");
                }
                return;
            }

            if (coPilotNote != CoPilotKey || Time.time - lastLogTime >= 1.0f)
            {
                Main.Logger.Log($"Disegno texture: {coPilotNote}");
            }

            var halfSize = Main.Settings.CoPilotSize / 2;
            GUI.DrawTexture(
                new Rect(
                    Screen.width * Main.Settings.PositionX - halfSize,
                    Screen.height * Main.Settings.PositionY - halfSize,
                    Main.Settings.CoPilotSize,
                    Main.Settings.CoPilotSize
                ),
                texture,
                ScaleMode.ScaleToFit
            );

            var newKey = CoPilotConfig?.Length > currentWaypointIndex ? CoPilotConfig[currentWaypointIndex] : null;
            if (newKey != null && newKey != CoPilotKey)
            {
                CoPilotKey = newKey;
                LastTimestamp = Time.time;
                PlayAudioNote(newKey);
                Main.Logger.Log($"Riproduzione audio avviata per: {newKey}");
            }
        }

        public static void PlayAudioNote(string noteKey)
        {
            Main.Logger.Log($"Tentativo di riprodurre audio: {noteKey}");
            if (!Main.Settings.EnableAudio || AudioClips == null || !AudioClips.ContainsKey(noteKey))
            {
                Main.Logger.Log($"[WARNING] Audio non riprodotto - EnableAudio: {Main.Settings.EnableAudio}, AudioClips: {(AudioClips != null ? "presente" : "null")}, Contiene chiave: {AudioClips?.ContainsKey(noteKey) ?? false}");
                return;
            }

            if (audioSource == null)
            {
                audioSource = new GameObject("CoPilotAudio").AddComponent<AudioSource>();
                audioSource.playOnAwake = false;
                audioSource.spatialBlend = 0f;    // 2D audio
                audioSource.priority = 88;         // Massima 
                audioSource.loop = false;         // Non ripetere
                audioSource.dopplerLevel = 0f;    // No effetto Doppler
                GameObject.DontDestroyOnLoad(audioSource.gameObject);
                Main.Logger.Log("AudioSource creato");
            }

            if (audioSource.isPlaying)
                audioSource.Stop();

            audioSource.clip = AudioClips[noteKey];
            audioSource.volume = Main.Settings.AudioVolume;
            Main.Logger.Log($"Riproduzione audio: {noteKey}, Volume: {audioSource.volume}, Clip duration: {audioSource.clip.length} secondi");
            audioSource.Play();
        }

        public static void Cleanup()
        {
            if (audioSource != null)
            {
                GameObject.Destroy(audioSource.gameObject);
                audioSource = null;
            }
        }
    }
}