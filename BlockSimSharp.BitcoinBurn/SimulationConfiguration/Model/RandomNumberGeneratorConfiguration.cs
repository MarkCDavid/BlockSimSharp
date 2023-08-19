namespace BlockSimSharp.BitcoinBurn.SimulationConfiguration.Model;

public sealed class RandomNumberGeneratorConfiguration
{
    public bool UseStaticSeed { get; init; }
    public int StaticSeed { get; init; }
}