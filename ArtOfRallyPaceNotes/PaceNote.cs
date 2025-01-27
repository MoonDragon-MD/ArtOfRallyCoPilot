using System.Collections.Generic;
using JetBrains.Annotations;
using ArtOfRallyPaceNotes.Settings;
using UnityEngine;
using UnityModManagerNet;

namespace ArtOfRallyPaceNotes
{
    public static class PaceNote
    {
        [CanBeNull] public static string[] PaceNoteConfig = null;

        [CanBeNull] public static Dictionary<string, Texture2D> Textures = null;
        [CanBeNull] public static Dictionary<string, AudioClip> AudioClips = null;
        private static AudioSource audioSource;
        private static string PaceNoteKey;
        private static float LastTimestamp;

        public static void Draw(UnityModManager.ModEntry modEntry)
        {
            var paceNote = PaceNoteConfig?[PaceNoteManager.CurrentWaypoint];

            if (Main.Settings.ShowCurrentWaypoint)
            {
                GUI.Label(
                    new Rect(0, 0, 200, 200),
                    $"Waypoint {PaceNoteManager.CurrentWaypoint}\nPace Note: {paceNote}"
                );
            }

            if (paceNote == null || Textures?.ContainsKey(paceNote) != true) return;
            var texture = Textures![paceNote];

            var halfSize = Main.Settings.PaceNoteSize / 2;

            GUI.DrawTexture(
                new Rect(
                    Screen.width * Main.Settings.PositionX - halfSize,
                    Screen.height * Main.Settings.PositionY - halfSize,
                    Main.Settings.PaceNoteSize,
                    Main.Settings.PaceNoteSize
                ),
                texture,
                ScaleMode.ScaleToFit
            );

            var newKey = PaceNoteConfig?[PaceNoteManager.CurrentWaypointIndex];
            if (newKey != null && newKey != PaceNoteKey)
            {
                PaceNoteKey = newKey;
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
                audioSource = new GameObject("PaceNoteAudio").AddComponent<AudioSource>();
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
