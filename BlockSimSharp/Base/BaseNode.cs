namespace BlockSimSharp.Base;

public abstract class BaseNode<TBlock, TTransaction>
    where TBlock: BaseBlock<TTransaction>, new()
    where TTransaction: BaseTransaction<TTransaction>
{
    public int Id { get; }
    public List<TBlock> BlockChain { get; }
    public List<TTransaction> TransactionPool { get; private set; }
    public int Blocks { get; set; }
    public float Balance { get; set; } 

    protected BaseNode(int id)
    {
        Id = id;
        //  2. Add Genesis Block to all Nodes
        BlockChain = new List<TBlock> { new() };
        TransactionPool = new List<TTransaction>();
        Blocks = 0;
        Balance = 0.0f;
    }

    public int BlockChainLength => BlockChain.Count - 1;
    public TBlock LastBlock => BlockChain[BlockChainLength];

    public virtual void UpdateLocalBlockChain(BaseNode<TBlock, TTransaction> sourceNode, int depth)
    {
        // Say NodeA has a BlockChain of [b0, b1, b2] and then they mine Block b3.
        // A receive event for NodeB is scheduled some time in the future.
        // Before the receive event is handled, NodeA mines Block b4. Their
        // copy of the BlockChain is [b0, b1, b2, b3, b4].
        // Eventually, the receive event for NodeB is handled. Say their copy
        // of the BlockChain is [b0] only. Our expectation is, that after the
        // update of local BlockChain, NodeB would have [b0, b1, b2, b3] and not
        // b4, as it was mined after propogation of the b3 happened.
        for (var index = 0; index < depth; index++)
        {
            if (index < BlockChain.Count)
                BlockChain[index] = sourceNode.BlockChain[index];
            else
                BlockChain.Add(sourceNode.BlockChain[index]);
        }

        if (Configuration.Instance.TransactionsEnabled && Configuration.Instance.TransactionContextType == "full")
        {
            UpdateTransactionPool(LastBlock);
        }
    }

    public void UpdateTransactionPool(TBlock block)
    {
        var blockTransactionIds = block.Transactions
            .Select(transaction => transaction.TransactionId)
            .ToHashSet();
        
        TransactionPool = TransactionPool
            .Where(transaction => !blockTransactionIds.Contains(transaction.TransactionId))
            .ToList();
    }
}