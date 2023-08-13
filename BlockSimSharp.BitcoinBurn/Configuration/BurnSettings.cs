using BlockSimSharp.Core.Configuration.Contracts;

namespace BlockSimSharp.BitcoinBurn.Configuration;

public class BurnSettings: ISettings
{
    public float AveragePowerCostPerSecond { get; init; }
    public float MaximumPowerCostVarianceInDecimalPercentage { get; init; }
    public float BitcoinCostPerDifficultyLevel { get; init; }
    public int DefaultHashDifficulty { get; init; }
    public int ExponentialBaseOfDifficulty { get; init; }
    public int ExponentialBaseOfDifficultyCost { get; init; }
}