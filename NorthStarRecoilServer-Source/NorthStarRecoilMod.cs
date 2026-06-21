using System.Text.Json;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Services;

namespace NorthStarRecoilServer;

[Injectable]
public sealed class NorthStarRecoilMod(DatabaseService databaseService) : IOnLoad
{
    public static int TypePriority => 999;

    private static readonly string ModPath =
        Path.Combine(Environment.CurrentDirectory, "user", "mods", "NorthStarRecoilServer");

    private static readonly string ConfigPath =
        Path.Combine(ModPath, "config.json");

    private static readonly string ResultsPath =
        Path.Combine(ModPath, "recoil-results.txt");

    public Task OnLoad()
    {

        var config = LoadConfig();

        var items = databaseService.GetItems();
        int patched = 0;

        foreach (var item in items.Values)
        {
            if (item.Properties?.WeapClass == null)
                continue;

            if (item.Properties.RecoilForceUp != null)
                item.Properties.RecoilForceUp *= config.VerticalRecoil;

            if (item.Properties.RecoilForceBack != null)
                item.Properties.RecoilForceBack *= config.HorizontalRecoil;

            if (item.Properties.RecoilCamera != null)
                item.Properties.RecoilCamera *= config.CameraRecoil;

            if (item.Properties.CameraSnap != null)
                item.Properties.CameraSnap *= config.CameraSnap;

            patched++;
        }

        File.WriteAllText(
            ResultsPath,
            $"NorthStar Recoil loaded. Weapons patched: {patched}\n" +
            $"Vertical Recoil: {config.VerticalRecoil}\n" +
            $"Horizontal Recoil: {config.HorizontalRecoil}\n" +
            $"Camera Recoil: {config.CameraRecoil}\n" +
            $"Camera Snap: {config.CameraSnap}");

        return Task.CompletedTask;
    }

    private static NorthStarConfig LoadConfig()
    {
        if (!File.Exists(ConfigPath))
        {
            var defaultConfig = new NorthStarConfig();
            SaveConfig(defaultConfig);
            return defaultConfig;
        }

        var json = File.ReadAllText(ConfigPath);

        return JsonSerializer.Deserialize<NorthStarConfig>(json)
               ?? new NorthStarConfig();
    }

    private static void SaveConfig(NorthStarConfig config)
    {
        File.WriteAllText(
            ConfigPath,
            JsonSerializer.Serialize(config, new JsonSerializerOptions
            {
                WriteIndented = true
            }));
    }

    private sealed class NorthStarConfig
    {
        public double VerticalRecoil { get; set; } = 0.35;
        public double HorizontalRecoil { get; set; } = 0.50;
        public double CameraRecoil { get; set; } = 0.25;
        public double CameraSnap { get; set; } = 0.25;
    }
}