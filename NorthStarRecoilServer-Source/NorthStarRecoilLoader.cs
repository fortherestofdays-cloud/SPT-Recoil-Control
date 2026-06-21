using SPTarkov.Server.Core.Models.Spt.Mod;

using SemVersion = SemanticVersioning.Version;
using SemRange = SemanticVersioning.Range;

namespace NorthStarRecoilServer;

public record NorthStarRecoilMetadata : AbstractModMetadata
{
    public override string ModGuid { get; init; } = "com.kraken.northstarrecoil";
    public override string Name { get; init; } = "NorthStarRecoilControl";
    public override string Author { get; init; } = "Kraken";

    public override List<string>? Contributors { get; init; } = null;
    public override List<string>? Incompatibilities { get; init; } = null;

    public override SemVersion Version { get; init; } = new SemVersion("1.0.0");
    public override SemRange SptVersion { get; init; } = new SemRange("~4.0.13");

    public override string License { get; init; } = "MIT";
    public override string Url { get; init; } = "";

    public override bool? IsBundleMod { get; init; } = false;

    public override Dictionary<string, SemRange> ModDependencies { get; init; } = new();
}