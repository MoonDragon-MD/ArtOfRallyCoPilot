using UnityEngine;
using UnityEngine.Networking;

public class AudioAssetLoader
{
    private static AudioClip LoadMP3(string path)
    {
        using (var uwr = UnityWebRequestMultimedia.GetAudioClip("file://" + path, AudioType.MPEG))
        {
            uwr.SendWebRequest();
            while (!uwr.isDone) { }
            if (uwr.isNetworkError || uwr.isHttpError)
            {
                Debug.LogError(uwr.error);
                return null;
            }
            return DownloadHandlerAudioClip.GetContent(uwr);
        }
    }
}


/* oppure potrei usare questo metodo:
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking; // Per UnityWebRequest e DownloadHandlerAudioClip

namespace ArtOfRallyCoPilots.Loader
{
    public static class AudioAssetLoader
    {
        private const string CoPilotAudioPath = "CoPilotAssets";
    
        public static Dictionary<string, AudioClip> LoadAudioAssets()
        {
            var directory = 
                new DirectoryInfo(Path.Combine(Main.ModEntry.Path, CoPilotAudioPath, Main.Settings.AudioSet));
            var audioClips = new Dictionary<string, AudioClip>();
        
            foreach (var file in directory.GetFiles("*.mp3"))
            {
                var audioClip = LoadMP3(file.FullName);
                audioClips[Path.GetFileNameWithoutExtension(file.Name)] = audioClip;
            }
        
            return audioClips;
        }

        private static AudioClip LoadMP3(string path)
        {
            using (var uwr = UnityWebRequestMultimedia.GetAudioClip("file://" + path, AudioType.MPEG))
            {
                uwr.SendWebRequest();
                while (!uwr.isDone) { }
                if (uwr.result == UnityWebRequest.Result.ConnectionError || uwr.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError(uwr.error);
                    return null;
                }
                return DownloadHandlerAudioClip.GetContent(uwr);
            }
        }
    }
}
*/