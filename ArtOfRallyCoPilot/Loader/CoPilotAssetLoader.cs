using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using ArtOfRallyCoPilot; // Aggiunto per accedere a Main

namespace ArtOfRallyCoPilot.Loader
{
    public static class CoPilotAssetLoader
    {
        private const string CoPilotAssetPath = "CoPilotAssets";

        public static Dictionary<string, Texture2D> LoadAssets()
        {
            var directory = new DirectoryInfo(Path.Combine(Main.ModEntry.Path, CoPilotAssetPath, Main.Settings.AssetSet));
            var textures = new Dictionary<string, Texture2D>();

            foreach (var file in directory.GetFiles("*.png"))
            {
                try
                {
                    var texture = new Texture2D(2, 2, TextureFormat.ARGB32, false);
                    if (texture.LoadImage(File.ReadAllBytes(file.FullName)))
                    {
                        texture.wrapMode = TextureWrapMode.Clamp;
                        textures[Path.GetFileNameWithoutExtension(file.Name)] = texture;
                        Main.Logger.Log($"Texture caricata: {file.Name}");
                    }
                    else
                    {
                        Main.Logger.Log($"[ERROR] Errore caricamento texture: {file.Name}");
                    }
                }
                catch (Exception e)
                {
                    Main.Logger.Log($"[ERROR] Eccezione durante caricamento texture {file.Name}: {e.Message}");
                }
            }

            Main.Logger.Log($"Loaded Asset Set {Main.Settings.AssetSet}");
            Main.Logger.Log($"Supported Pace Notes: {string.Join(", ", textures.Keys)}");

            return textures;
        }
    }
}