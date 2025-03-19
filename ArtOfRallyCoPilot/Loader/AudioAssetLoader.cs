using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

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
        
            foreach (var file in directory.GetFiles("*.wav"))
            {
                var audioClip = LoadWAV(file.FullName);
                audioClips[Path.GetFileNameWithoutExtension(file.Name)] = audioClip;
            }
        
            return audioClips;
        }

        private static AudioClip LoadWAV(string path)
        {
            byte[] fileData = File.ReadAllBytes(path);
            WAV wav = new WAV(fileData);
            AudioClip audioClip = AudioClip.Create(Path.GetFileNameWithoutExtension(path), wav.SampleCount, wav.ChannelCount, wav.Frequency, false);
            audioClip.SetData(wav.LeftChannel, 0);
            return audioClip;
        }
    }

    public class WAV
    {
        public float[] LeftChannel { get; private set; }
        public float[] RightChannel { get; private set; }
        public int ChannelCount { get; private set; }
        public int SampleCount { get; private set; }
        public int Frequency { get; private set; }

        public WAV(byte[] fileData)
        {
            // Legge i dati del file WAV e li converte in float[]
            // Implementa la logica per leggere i dati del file WAV

            // Esempio semplificato di parsing del file WAV:
            ChannelCount = BitConverter.ToInt16(fileData, 22);
            Frequency = BitConverter.ToInt32(fileData, 24);
            int pos = 44; // Inizio dei dati audio
            int samples = (fileData.Length - pos) / 2; // 2 byte per campione (16 bit)
            SampleCount = samples / ChannelCount;

            LeftChannel = new float[SampleCount];
            RightChannel = ChannelCount == 2 ? new float[SampleCount] : null;

            int i = 0;
            while (pos < fileData.Length)
            {
                LeftChannel[i] = BitConverter.ToInt16(fileData, pos) / 32768.0f;
                pos += 2;
                if (ChannelCount == 2)
                {
                    RightChannel[i] = BitConverter.ToInt16(fileData, pos) / 32768.0f;
                    pos += 2;
                }
                i++;
            }
        }
    }
}