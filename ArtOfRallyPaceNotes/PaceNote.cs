using System.Collections.Generic;
using JetBrains.Annotations;
using ArtOfRallyPaceNotes.Settings;
using UnityEngine;
using UnityModManagerNet;
using Color = UnityEngine.Color;
using FontStyle = UnityEngine.FontStyle;

namespace ArtOfRallyPaceNotes
{
    public static class PaceNote
    {
        [CanBeNull] public static string[] PaceNoteConfig = null;

        [CanBeNull] public static Dictionary<string, Texture2D> Textures = null;
        [CanBeNull] public static Dictionary<string, AudioClip> AudioClips = null;
        private static AudioSource audioSource;

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
        }
        public static void PlayAudioNote(string noteKey)
        {
            if (!Main.Settings.EnableAudio || AudioClips == null || !AudioClips.ContainsKey(noteKey))
                return;
            
            if (audioSource == null)
            {
                audioSource = new GameObject("PaceNoteAudio").AddComponent<AudioSource>();
                audioSource.playOnAwake = false;
                audioSource.spatialBlend = 0f;    // 2D audio (not spatialized)
                audioSource.priority = 0;         // Highest priority
                audioSource.loop = false;         // Don't repeat the audio
                audioSource.dopplerLevel = 0f;    // Disable Doppler effect
                GameObject.DontDestroyOnLoad(audioSource.gameObject);
            }
            
            // Interrupts the previous audio if he is still playing
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
    
        public static void Draw(UnityModManager.ModEntry modEntry)
        {
            var newKey = PaceNoteConfig?[PaceNoteManager.CurrentWaypointIndex];
            if (newKey != null && newKey != PaceNoteKey)
            {
                PaceNoteKey = newKey;
                LastTimestamp = Time.time;
                PlayAudioNote(newKey); // Reproduce the audio when the note changes
            }
    }
}
