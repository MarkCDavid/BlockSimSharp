namespace BlockSimSharp.BitcoinBurn.SimulationConfiguration.Model;

public sealed class BlockConfiguration
{
    public double Reward { get; init; }
    public int AverageIntervalInSeconds { get; init; }
    public double AveragePropogationDelay { get; init; }
}