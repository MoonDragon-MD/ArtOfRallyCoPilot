using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace ArtOfRallyPaceNotes.Loader
{
    public static class AudioAssetLoader
    {
        private const string PaceNoteAudioPath = "PaceNoteAssets";
    
        public static Dictionary<string, AudioClip> LoadAudioAssets()
        {
            var directory = 
			    new DirectoryInfo(Path.Combine(Main.ModEntry.Path, PaceNoteAudioPath, Main.Settings.AudioSet));
            var audioClips = new Dictionary<string, AudioClip>();
        
            foreach (var file in directory.GetFiles("*.mp3"))
            {
                var audioClip = LoadMP3(file.FullName);
                audioClips[Path.GetFileNameWithoutExtension(file.Name)] = audioClip;
            }
        
            return audioClips;
        }
    }
}