using BlockSimSharp.Core.Configuration.Contracts;

namespace BlockSimSharp.Core.Configuration.Model;

public sealed class RandomNumberGenerationSettings: ISettings
{
    public bool UseStaticSeed { get; init; }
    public int StaticSeed { get; init; }
}