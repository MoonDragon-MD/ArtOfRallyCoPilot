using System.Collections.Generic;
using JetBrains.Annotations;
using ArtOfRallyCoPilots.Settings;
using UnityEngine;
using UnityModManagerNet;

namespace ArtOfRallyCoPilots
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
            var CoPilot = CoPilotConfig?[CoPilotManager.CurrentWaypointIndex]; // Modificato da CurrentWaypoint a CurrentWaypointIndex

            if (Main.Settings.ShowCurrentWaypoint)
            {
                GUI.Label(
                    new Rect(0, 0, 200, 200),
                    $"Waypoint {CoPilotManager.CurrentWaypointIndex}\nCoPilot: {CoPilot}"
                );
            }

            if (CoPilot == null || Textures?.ContainsKey(CoPilot) != true) return;
            var texture = Textures![CoPilot];

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
                PlayAudioNote(newKey); // Reproduce l'audio quando cambia la nota
            }
        }

        public static void PlayAudioNote(string noteKey)
        {
            if (!Main.Settings.EnableAudio || AudioClips == null || !AudioClips.ContainsKey(noteKey))
                return;
            
            if (audioSource == null)
            {
                audioSource = new GameObject("CoPilotAudio").AddComponent<AudioSource>();
                audioSource.playOnAwake = false;
                audioSource.spatialBlend = 0f;    // 2D audio (non spazializzato)
                audioSource.priority = 0;         // Massima priorità
                audioSource.loop = false;         // Non ripetere l'audio
                audioSource.dopplerLevel = 0f;    // Disabilitare effetto Doppler
                GameObject.DontDestroyOnLoad(audioSource.gameObject);
            }
            
            // Interrompi l'audio precedente se è ancora in riproduzione
            if (audioSource.isPlaying)
                audioSource.Stop();
                
            audioSource.clip = AudioClips[noteKey];
            audioSource.volume = Main.Settings.AudioVolume;
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