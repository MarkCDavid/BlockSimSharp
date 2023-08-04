namespace BlockSimSharp.BitcoinBurn.Simulation.Statistics;

public class ProfitStatistics
{
    public int NodeId { get; set; }
    public int Blocks { get; set; }

    public float PercentageOfAllBlocks { get; set; }
    public float Balance { get; set; }
    public float TotalPowerCostInDollars { get; set; }
    public float TotalDifficultyReductionCostInDollars { get; set; }
    public float HashPower { get; set; }
    public float PercentageOfAllHashPower { get; set; }
}