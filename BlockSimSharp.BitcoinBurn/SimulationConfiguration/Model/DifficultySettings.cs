namespace BlockSimSharp.BitcoinBurn.SimulationConfiguration.Model;

public class DifficultySettings
{
    public int AdjustmentFrequencyInBlocks { get; set; }
    public bool DecreaseByBurnEnabled { get; set; }
    public double ProbabilityForParticipationThisEpoch { get; set; }
    
    public double ReductionHighBound { get; set; }
    public double ReductionLowBound { get; set; }
}