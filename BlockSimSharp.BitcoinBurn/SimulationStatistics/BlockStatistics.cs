namespace BlockSimSharp.BitcoinBurn.SimulationStatistics;

public class BlockStatistics
{
    public int Depth { get; set; }
    public int BlockId { get; set; }
    public int? PreviousBlockId { get; set; }
    public double Timestamp { get; set; }
    public int TransactionCount { get; set; }
    public double SizeInMb { get; set; }
}