namespace BlockSimSharp.BitcoinBurn.SimulationConfiguration.Model;

public class DifficultyConfiguration
{
    public int AdjustmentFrequencyInBlocks { get; set; }
    public bool DecreaseByBurnEnabled { get; set; }
    public double ProbabilityForParticipationThisEpoch { get; set; }
    
    public double DecreaseAmountHighBound { get; set; }
    public double DecreaseAmountLowBound { get; set; }
}