using BlockSimSharp.Base;

namespace BlockSimSharp;

public class LightTransactionContext<TNode, TBlock, TTransaction>: ITransactionContext<TNode, TBlock, TTransaction>
    where TNode: BaseNode<TBlock, TTransaction>
    where TBlock: BaseBlock<TTransaction>, new()
    where TTransaction: BaseTransaction
{
    private List<TTransaction> LocalTransactionPool { get; set; }
    public LightTransactionContext()
    {
        LocalTransactionPool = new List<TTransaction>();
    }


    public (List<TTransaction>, float) CollectTransactions(TNode node, float currentTime)
    {
        var transactions = new List<TTransaction>();
        var blockSizeInMb = 0.0f;

        foreach (var transaction in LocalTransactionPool.OrderByDescending(transaction => transaction.Fee))
        {
            if (blockSizeInMb + transaction.SizeInMb >= Configuration.Instance.BlockSizeInMb) 
                continue;
            
            blockSizeInMb += transaction.SizeInMb;
            transactions.Add(transaction);
        }

        return (transactions, blockSizeInMb);
    }
}