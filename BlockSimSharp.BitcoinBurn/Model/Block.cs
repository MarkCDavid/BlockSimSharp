using BlockSimSharp.BitcoinBurn.Simulation;

namespace BlockSimSharp.BitcoinBurn.Model;

public class  Block
{
    public int BlockId { get; init; }
    public Block? PreviousBlock { get; init; }
    public Node? Miner { get; init; }
    public int Depth { get; init;  }
    public double MinedAt { get; init; }
    public double ScheduledAt { get; init; }
}