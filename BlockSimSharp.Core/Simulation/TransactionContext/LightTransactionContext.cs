using BlockSimSharp.Core.Configuration.Model;
using BlockSimSharp.Core.Model;

namespace BlockSimSharp.Core.Simulation.TransactionContext;

public sealed class LightTransactionContext<TTransaction, TBlock, TNode, TNetwork, TConsensus, TEvent, TEventQueue, TScheduler, TContext>
    : BaseTransactionContext<TTransaction, TBlock, TNode, TNetwork, TConsensus, TEvent, TEventQueue, TScheduler, TContext>
    where TTransaction: BaseTransaction<TTransaction>, new()
    where TBlock: BaseBlock<TTransaction, TBlock, TNode>, new()
    where TNode: BaseNode<TTransaction, TBlock, TNode>
    where TNetwork: BaseNetwork<TTransaction, TBlock, TNode, TNetwork, TConsensus, TEvent, TEventQueue, TScheduler, TContext>
    where TConsensus: BaseConsensus<TTransaction, TBlock, TNode, TNetwork, TConsensus, TEvent, TEventQueue, TScheduler, TContext>
    where TScheduler: BaseScheduler<TTransaction, TBlock, TNode, TNetwork, TConsensus, TEvent, TEventQueue, TScheduler, TContext>
    where TEventQueue: BaseEventQueue<TTransaction, TBlock, TNode, TNetwork, TConsensus, TEvent, TEventQueue, TScheduler, TContext>
    where TEvent: BaseEvent<TTransaction, TBlock, TNode, TNetwork, TConsensus, TEvent, TEventQueue, TScheduler, TContext>
    where TContext: BaseContext<TTransaction, TBlock, TNode, TNetwork, TConsensus, TEvent, TEventQueue, TScheduler, TContext>
{
    private List<TTransaction> LocalTransactionPool { get; set; }
    public LightTransactionContext()
    {
        LocalTransactionPool = new List<TTransaction>();
    }

    public override void CreateTransactions(TContext context)
    {
        var blockSettings = context.Settings.Get<BlockSettings>();
        var transactionSettings = context.Settings.Get<TransactionSettings>();
        var transactionCountInBlock = transactionSettings.RatePerSecond * blockSettings.AverageIntervalInSeconds;
        
        LocalTransactionPool = Enumerable.Range(0, transactionCountInBlock)
            .Select(_ => new TTransaction()
            {
                TransactionId = context.Random.Next(),
                SenderNodeId = context.Nodes.ElementAt(context.Random.Next(context.Nodes.Count)).NodeId,
                ReceiverNodeId = context.Nodes.ElementAt(context.Random.Next(context.Nodes.Count)).NodeId,
                SizeInMb = Utility.Expovariate(1 / transactionSettings.AverageSizeInMb),
                Fee = Utility.Expovariate(1 / transactionSettings.AverageFee)
            }).ToList();
    }

    public override (List<TTransaction>, float) CollectTransactions(TContext context, TNode node, float currentTime)
    {
        var blockSettings = context.Settings.Get<BlockSettings>();
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