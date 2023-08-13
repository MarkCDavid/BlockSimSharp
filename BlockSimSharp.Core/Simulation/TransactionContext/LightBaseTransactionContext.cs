using BlockSimSharp.Core.Configuration;
using BlockSimSharp.Core.Configuration.Model;
using BlockSimSharp.Core.Model;

namespace BlockSimSharp.Core.Simulation.TransactionContext;

public sealed class LightBaseTransactionContext<TTransaction, TBlock, TNode>: BaseTransactionContext<TTransaction, TBlock, TNode>
    where TTransaction: BaseTransaction<TTransaction, TBlock, TNode>, new()
    where TBlock: BaseBlock<TTransaction, TBlock, TNode>, new()
    where TNode: BaseNode<TTransaction, TBlock, TNode>
{
    private List<TTransaction> LocalTransactionPool { get; set; }
    public LightBaseTransactionContext()
    {
        LocalTransactionPool = new List<TTransaction>();
    }

    public override void CreateTransactions(SimulationContext context)
    {
        var nodes = context.Get<BaseNodes<TTransaction, TBlock, TNode>>();
        var randomness = context.Get<Randomness>();

        var settings = context.Get<Settings>();
        
        var blockSettings = settings.Get<BlockSettings>();
        var transactionSettings = settings.Get<TransactionSettings>();
        var transactionCountInBlock = transactionSettings.RatePerSecond * blockSettings.AverageIntervalInSeconds;
        
        LocalTransactionPool = Enumerable.Range(0, transactionCountInBlock)
            .Select(_ => new TTransaction()
            {
                TransactionId = randomness.Next(),
                SenderNode = nodes.ElementAt(randomness.Next(nodes.Count)),
                ReceiverNode = nodes.ElementAt(randomness.Next(nodes.Count)),
                SizeInMb = randomness.Expovariate(1 / transactionSettings.AverageSizeInMb),
                Fee = randomness.Expovariate(1 / transactionSettings.AverageFee)
            }).ToList();
    }

    public override (List<TTransaction>, float) CollectTransactions(SimulationContext context, TNode node, float currentTime)
    {
        var settings = context.Get<Settings>();
        var blockSettings = settings.Get<BlockSettings>();
        
        var transactions = new List<TTransaction>();
        var blockSizeInMb = 0.0f;
        
        foreach (var transaction in LocalTransactionPool.OrderByDescending(transaction => transaction.Fee))
        {
            if (blockSizeInMb + transaction.SizeInMb >= blockSettings.SizeInMb) 
                continue;
            
            blockSizeInMb += transaction.SizeInMb;
            transactions.Add(transaction);
        }
        
        return (transactions, blockSizeInMb);
    }
}