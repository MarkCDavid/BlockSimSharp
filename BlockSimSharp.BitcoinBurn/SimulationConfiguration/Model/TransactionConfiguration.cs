using BlockSimSharp.Core.Configuration.Enum;

namespace BlockSimSharp.BitcoinBurn.SimulationConfiguration.Model;

public sealed class TransactionConfiguration
{
    public bool Enabled { get; init; }
    public TransactionContextType Type { get; init; }
    public int RatePerSecond { get; init; }
    public double AverageFee { get; init; }
    public double AverageSizeInMb { get; init; }
    public double AveragePropogationDelay { get; init; }
}