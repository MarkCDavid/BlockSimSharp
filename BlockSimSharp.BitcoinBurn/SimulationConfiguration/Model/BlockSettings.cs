namespace BlockSimSharp.BitcoinBurn.SimulationConfiguration.Model;

public sealed class BlockSettings
{
    public double Reward { get; init; }
    public int AverageIntervalInSeconds { get; init; }
    public double AveragePropogationDelay { get; init; }
}