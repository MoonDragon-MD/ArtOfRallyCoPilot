using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace ArtOfRallyCoPilots.Loader
{
    public static class CoPilotAssetLoader
    {
        private const string CoPilotAssetPath = "CoPilotAssets";

        public static Dictionary<string, Texture2D> LoadAssets()
        {
            var directories =
                new DirectoryInfo(Path.Combine(Main.ModEntry.Path, CoPilotAssetPath, Main.Settings.AssetSet))
                    .GetFiles();
            var textures = new Dictionary<string, Texture2D>();

            foreach (var file in directories)
            {
                var texture = new Texture2D(2, 2, TextureFormat.ARGB32, false);
                texture.LoadImage(File.ReadAllBytes(file.FullName));
                texture.wrapMode = TextureWrapMode.Clamp;

                textures[Path.GetFileNameWithoutExtension(file.Name)] = texture;
            }
            
            Main.Logger.Log($"Loaded Asset Set {Main.Settings.AssetSet}");
            Main.Logger.Log($"Supported Pace Notes: {string.Join(", ", textures.Keys)}");

            return textures;
        }
    }
}
