using BlockSimSharp.Base;

namespace BlockSimSharp;

public class LightTransactionContext<TNode, TBlock, TTransaction>: ITransactionContext<TNode, TBlock, TTransaction>
    where TNode: BaseNode<TBlock, TTransaction>
    where TBlock: BaseBlock<TTransaction>, new()
    where TTransaction: BaseTransaction, new()
{
    private List<TTransaction> LocalTransactionPool { get; set; }
    public LightTransactionContext()
    {
        LocalTransactionPool = new List<TTransaction>();
    }

    public void CreateTransactions(SimulationContext<TNode, TBlock, TTransaction> context)
    {
        var random = new Random();
        var transactionCountInBlock = Configuration.Instance.TransactionsPerSecond * Configuration.Instance.AverageBlockIntervalInSeconds;

        LocalTransactionPool = Enumerable.Range(0, transactionCountInBlock)
            .Select(_ => new TTransaction()
            {
                TransactionId = random.Next(),
                SenderNodeId = context.Nodes.ElementAt(random.Next(context.Nodes.Count)).Id,
                ReceiverNodeId = context.Nodes.ElementAt(random.Next(context.Nodes.Count)).Id,
                SizeInMb = Utility.Expovariate(1 / Configuration.Instance.AverageTransactionSizeInMb),
                Fee = Utility.Expovariate(1 / Configuration.Instance.AverageTransactionFee)
            }).ToList();
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