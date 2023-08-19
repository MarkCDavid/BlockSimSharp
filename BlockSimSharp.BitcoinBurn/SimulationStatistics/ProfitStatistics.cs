namespace BlockSimSharp.BitcoinBurn.SimulationStatistics;

public class ProfitStatistics
{
    public int NodeId { get; set; }
    public int Blocks { get; set; }

    public double PercentageOfAllBlocks { get; set; }
    public double Balance { get; set; }
    public double TotalPowerCost { get; set; }
    public double TotalDifficultyReductionCostInBitcoins { get; set; }
    public double HashPower { get; set; }
    public double PercievedHashPower { get; set; }
    public double PercentageOfAllHashPower { get; set; }
}