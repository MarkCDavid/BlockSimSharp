namespace BlockSimSharp.Core.Model;

public abstract class BaseBlock<TTransaction>
    where TTransaction: BaseTransaction<TTransaction>
{
    protected BaseBlock()
    {
        Transactions = new List<TTransaction>();
    }

    public int BlockId { get; set; }
    
    public int? PreviousBlockId { get; set; }
    
    public int MinerId { get; set; }
    
    
    public int Depth { get; set;  }
    
    
    public float Timestamp { get; set; }
    public List<TTransaction> Transactions { get; set; }
}