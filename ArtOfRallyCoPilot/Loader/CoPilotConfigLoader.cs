using System;
using System.IO;
using JetBrains.Annotations;

namespace ArtOfRallyCoPilots.Loader
{
    public static class CoPilotConfigLoader
    {
        private const string CoPilotConfigPath = "CoPilotConfigs";

        [ItemCanBeNull]
        public static string[] LoadCoPilotConfig(string stage, int count)
        {
            var path = Path.Combine(Main.ModEntry.Path, CoPilotConfigPath, Main.Settings.ConfigSet, $"{stage}.csv");

            if (!File.Exists(path))
            {
                Main.Logger.Warning($"No config file found for stage {stage} (tried set {Main.Settings.ConfigSet})");
                return null;
            }

            var outList = new string[count];

            try
            {
                foreach (var line in File.ReadLines(path))
                {
                    var entries = line.Split(',');
                    var start = int.Parse(entries[0]);
                    var end = int.Parse(entries[1]);

                    for (var i = start; i < end; i++)
                    {
                        outList[i] = entries[2];
                    }
                }

                Main.Logger.Log("Pace notes loaded");
            }
            catch (Exception e)
            {
                Main.Logger.Error(e.Message);
            }

            return outList;
        }
    }
}
