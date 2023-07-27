using BlockSimSharp.Core.Configuration.Model;
using BlockSimSharp.Core.Model;

namespace BlockSimSharp.Core.Simulation.TransactionContext;

public sealed class FullTransactionContext<TTransaction, TBlock, TNode, TNetwork, TConsensus, TEvent, TScheduler, TContext>
    : BaseTransactionContext<TTransaction, TBlock, TNode, TNetwork, TConsensus, TEvent, TScheduler, TContext>
    where TTransaction: BaseTransaction<TTransaction>, new()
    where TBlock: BaseBlock<TTransaction, TBlock, TNode>, new()
    where TNode: BaseNode<TTransaction, TBlock, TNode>
    where TNetwork: BaseNetwork<TTransaction, TBlock, TNode, TNetwork, TConsensus, TEvent, TScheduler, TContext>
    where TConsensus: BaseConsensus<TTransaction, TBlock, TNode, TNetwork, TConsensus, TEvent, TScheduler, TContext>
    where TScheduler: BaseScheduler<TTransaction, TBlock, TNode, TNetwork, TConsensus, TEvent, TScheduler, TContext>
    where TEvent: BaseEvent<TTransaction, TBlock, TNode, TNetwork, TConsensus, TEvent, TScheduler, TContext>
    where TContext: BaseContext<TTransaction, TBlock, TNode, TNetwork, TConsensus, TEvent, TScheduler, TContext>
{

    public override void CreateTransactions(TContext context)
    {
        var transactionSettings = context.Settings.Get<TransactionSettings>();
        var simulationSettings = context.Settings.Get<SimulationSettings>();
        
        var transactionCountDuringSimulation = transactionSettings.RatePerSecond * simulationSettings.LengthInSeconds;
        
        for (var index = 0; index < transactionCountDuringSimulation; index++)
        {
            var sender = context.Nodes.ElementAt(context.Random.Next(context.Nodes.Count));
        
            var transaction = new TTransaction()
            {
                TransactionId = context.Random.Next(),
                SenderNodeId = sender.NodeId,
                ReceiverNodeId = context.Nodes.ElementAt(context.Random.Next(context.Nodes.Count)).NodeId,
                TransactionCreateTime = context.Random.Next(0, simulationSettings.LengthInSeconds - 1),
                SizeInMb = Utility.Expovariate(1 / transactionSettings.AverageSizeInMb),
                Fee = Utility.Expovariate(1 / transactionSettings.AverageFee)
            };
            
            sender.TransactionPool.Add(transaction);
            PropogateTransaction(context, transaction);
        }
    }

    public override (List<TTransaction>, float) CollectTransactions(TContext context, TNode node, float currentTime)
    {
        var blockSettings = context.Settings.Get<BlockSettings>();
        var transactions = new List<TTransaction>();
        var blockSizeInMb = 0.0f;
        
        foreach (var transaction in node.TransactionPool.Where(transaction => transaction.TransactionReceiveTime < currentTime).OrderByDescending(transaction => transaction.Fee))
        {
            if (blockSizeInMb + transaction.SizeInMb >= blockSettings.SizeInMb) 
                continue;
            
            blockSizeInMb += transaction.SizeInMb;
            transactions.Add(transaction);
        }
        
        return (transactions, blockSizeInMb);
    }

    private void PropogateTransaction(TContext context, TTransaction transaction)
    {
        foreach (var receiverNode in context.Nodes.Where(node => node.NodeId != transaction.SenderNodeId))
        {
            var clonedTransaction = transaction.DeepClone();
            clonedTransaction.TransactionReceiveTime = transaction.TransactionCreateTime + context.Network.TransactionPropogationDelay(context);
            receiverNode.TransactionPool.Add(clonedTransaction);
        }
    }
}