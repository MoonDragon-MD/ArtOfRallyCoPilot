using System.Collections.Generic;
using JetBrains.Annotations;
using ArtOfRallyCoPilot.Settings;
using UnityEngine;
using UnityModManagerNet;
using ArtOfRallyCoPilot; // Aggiunto per accedere a Main

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

        public static void Draw(UnityModManager.ModEntry modEntry)
        {
            var CoPilot = CoPilotConfig?[CoPilotManager.CurrentWaypointIndex];
            Main.Logger.Log($"Waypoint corrente: {CoPilotManager.CurrentWaypointIndex}, Nota: {CoPilot ?? "null"}");

            if (Main.Settings.ShowCurrentWaypoint)
            {
                GUI.Label(
                    new Rect(0, 0, 200, 200),
                    $"Waypoint {CoPilotManager.CurrentWaypointIndex}\nCoPilot: {CoPilot}"
                );
            }

            if (CoPilot == null || Textures?.ContainsKey(CoPilot) != true)
            {
                Main.Logger.Log($"[WARNING] Texture non trovata per nota: {CoPilot}");
                return;
            }

            var texture = Textures![CoPilot];
            if (texture == null)
            {
                Main.Logger.Log($"[ERROR] Texture nulla per nota: {CoPilot}");
                return;
            }
            Main.Logger.Log($"Disegno texture: {CoPilot}");

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

            var newKey = CoPilotConfig?[CoPilotManager.CurrentWaypointIndex];
            if (newKey != null && newKey != CoPilotKey)
            {
                CoPilotKey = newKey;
                LastTimestamp = Time.time;
                PlayAudioNote(newKey);
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
                audioSource.priority = 0;         // Massima priorità
                audioSource.loop = false;         // Non ripetere
                audioSource.dopplerLevel = 0f;    // No effetto Doppler
                GameObject.DontDestroyOnLoad(audioSource.gameObject);
            }

            if (audioSource.isPlaying)
                audioSource.Stop();

            audioSource.clip = AudioClips[noteKey];
            audioSource.volume = Main.Settings.AudioVolume;
            Main.Logger.Log($"Riproduzione audio: {noteKey}, Volume: {audioSource.volume}");
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