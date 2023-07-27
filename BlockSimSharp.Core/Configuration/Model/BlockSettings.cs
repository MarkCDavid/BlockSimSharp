using BlockSimSharp.Core.Configuration.Contracts;
using BlockSimSharp.Core.Configuration.Enum;

namespace BlockSimSharp.Core.Configuration.Model;

public sealed class BlockSettings: ISettings
{
    public float SizeInMb { get; init; }
    public float Reward { get; init; }
    public int AverageIntervalInSeconds { get; init; }
    public float AveragePropogationDelay { get; init; }
}