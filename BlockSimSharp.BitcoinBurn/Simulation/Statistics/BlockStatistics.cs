namespace BlockSimSharp.BitcoinBurn.Simulation.Statistics;

public class BlockStatistics
{
    public int Depth { get; set; }
    public int BlockId { get; set; }
    public int? PreviousBlockId { get; set; }
    public float Timestamp { get; set; }
    public int TransactionCount { get; set; }
    public float SizeInMb { get; set; }
}