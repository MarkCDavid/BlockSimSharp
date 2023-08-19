namespace BlockSimSharp.Model;

public class Block
{
    public int BlockId { get; set; }
    public Block? PreviousBlock { get; set; }
    public Node? Miner { get; set; }
    public int Depth { get; set;  }
    public double ExecutedAt { get; set; }
    public double ScheduledAt { get; set; }
}