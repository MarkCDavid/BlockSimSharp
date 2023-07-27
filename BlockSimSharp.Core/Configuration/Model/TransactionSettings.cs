using BlockSimSharp.Core.Configuration.Contracts;
using BlockSimSharp.Core.Configuration.Enum;

namespace BlockSimSharp.Core.Configuration.Model;

public sealed class TransactionSettings: ISettings
{
    public bool Enabled { get; init; }
    public TransactionContextType Type { get; init; }
    public int RatePerSecond { get; init; }
    public float AverageFee { get; init; }
    public float AverageSizeInMb { get; init; }
    public float AveragePropogationDelay { get; init; }
}