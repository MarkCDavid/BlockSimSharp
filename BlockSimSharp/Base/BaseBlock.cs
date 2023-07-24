namespace BlockSimSharp.Base;

public abstract class BaseBlock
{
    public int BlockId { get; set; }
    public int Depth { get; set;  }
    public int? PreviousBlockId { get; set; }
    public float Timestamp { get; set; }
    public int MinerId { get; set; }
    public int Transactions { get; set; }
    public float SizeInMb { get; set; }
}