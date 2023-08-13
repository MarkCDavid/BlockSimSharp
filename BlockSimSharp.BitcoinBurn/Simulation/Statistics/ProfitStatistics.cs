namespace BlockSimSharp.BitcoinBurn.Simulation.Statistics;

public class ProfitStatistics
{
    public int NodeId { get; set; }
    public int Blocks { get; set; }

    public float PercentageOfAllBlocks { get; set; }
    public float Balance { get; set; }
    public float TotalPowerCost { get; set; }
    public float TotalDifficultyReductionCostInBitcoins { get; set; }
    public float HashPower { get; set; }
    public float PercievedHashPower { get; set; }
    public float PercentageOfAllHashPower { get; set; }
}