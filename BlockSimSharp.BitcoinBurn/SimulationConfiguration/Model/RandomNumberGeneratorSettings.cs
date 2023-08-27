namespace BlockSimSharp.BitcoinBurn.SimulationConfiguration.Model;

public sealed class RandomNumberGeneratorSettings
{
    public bool UseStaticSeed { get; init; }
    public int StaticSeed { get; init; }
    public bool HashPowerUseStaticSeed { get; init; }
    public int HashPowerStaticSeed { get; init; }
}