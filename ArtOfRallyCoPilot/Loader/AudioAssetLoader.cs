using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using ArtOfRallyCoPilot; // Aggiunto per accedere a Main

namespace ArtOfRallyCoPilot.Loader
{
    public static class AudioAssetLoader
    {
        private const string CoPilotAudioPath = "CoPilotAssets";

        public static Dictionary<string, AudioClip> LoadAudioAssets()
        {
            var directory = new DirectoryInfo(Path.Combine(Main.ModEntry.Path, CoPilotAudioPath, Main.Settings.AudioSet));
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
            if (fileData.Length < 44) throw new Exception("File WAV troppo corto o corrotto.");

            if (fileData[0] != 'R' || fileData[1] != 'I' || fileData[2] != 'F' || fileData[3] != 'F')
                throw new Exception("File non è un WAV valido.");

            ChannelCount = BitConverter.ToInt16(fileData, 22);
            Frequency = BitConverter.ToInt32(fileData, 24);
            int bitsPerSample = BitConverter.ToInt16(fileData, 34);
            int pos = 12;

            while (pos < fileData.Length - 4)
            {
                if (fileData[pos] == 'd' && fileData[pos + 1] == 'a' && fileData[pos + 2] == 't' && fileData[pos + 3] == 'a')
                {
                    pos += 4;
                    break;
                }
                pos++;
            }

            if (pos >= fileData.Length) throw new Exception("Chunk dei dati audio non trovato.");

            int dataSize = BitConverter.ToInt32(fileData, pos - 4);
            pos += 4;

            int bytesPerSample = bitsPerSample / 8;
            SampleCount = dataSize / (ChannelCount * bytesPerSample);

            LeftChannel = new float[SampleCount];
            RightChannel = ChannelCount == 2 ? new float[SampleCount] : null;

            for (int i = 0; i < SampleCount; i++)
            {
                int samplePos = pos + (i * ChannelCount * bytesPerSample);
                if (samplePos + bytesPerSample > fileData.Length) break;

                if (bitsPerSample == 16)
                {
                    short sample = BitConverter.ToInt16(fileData, samplePos);
                    LeftChannel[i] = sample / 32768.0f;

                    if (ChannelCount == 2 && samplePos + 2 < fileData.Length)
                    {
                        short rightSample = BitConverter.ToInt16(fileData, samplePos + 2);
                        RightChannel[i] = rightSample / 32768.0f;
                    }
                }
                else if (bitsPerSample == 8)
                {
                    byte sample = fileData[samplePos];
                    LeftChannel[i] = (sample - 128) / 128.0f;

                    if (ChannelCount == 2 && samplePos + 1 < fileData.Length)
                    {
                        byte rightSample = fileData[samplePos + 1];
                        RightChannel[i] = (rightSample - 128) / 128.0f;
                    }
                }
            }
        }
    }
}