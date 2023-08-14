using BlockSimSharp.Core.Configuration.Contracts;

namespace BlockSimSharp.BitcoinBurn.Configuration;

public class DifficultySettings: ISettings
{
    public int DifficultyAdjustmentFrequencyInBlocks { get; set; }
}