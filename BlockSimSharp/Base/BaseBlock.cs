namespace BlockSimSharp.Base;

public abstract class BaseBlock<TTransaction>
    where TTransaction: BaseTransaction
{
    public int BlockId { get; set; }
    public int Depth { get; set;  }
    public int? PreviousBlockId { get; set; }
    public float Timestamp { get; set; }
    public int MinerId { get; set; }
    public List<TTransaction> Transactions { get; set; }
    public float SizeInMb { get; set; }
}