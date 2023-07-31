namespace BlockSimSharp.Core.Model;

public abstract class BaseBlock<TTransaction, TBlock, TNode>
    where TTransaction: BaseTransaction<TTransaction, TBlock, TNode>
    where TBlock: BaseBlock<TTransaction, TBlock, TNode>, new()
    where TNode: BaseNode<TTransaction, TBlock, TNode>
{
    protected BaseBlock()
    {
        Transactions = new List<TTransaction>();
    }

    public int BlockId { get; set; }
    
    public TBlock? PreviousBlock { get; set; }
    
    public TNode? Miner { get; set; }
    
    
    public int Depth { get; set;  }
    
    public float SizeInMb { get; set; }
    
    
    public float Timestamp { get; set; }
    public List<TTransaction> Transactions { get; set; }
}