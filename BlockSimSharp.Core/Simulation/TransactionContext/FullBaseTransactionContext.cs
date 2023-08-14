using BlockSimSharp.Core.Configuration;
using BlockSimSharp.Core.Configuration.Model;
using BlockSimSharp.Core.Model;

namespace BlockSimSharp.Core.Simulation.TransactionContext;

public sealed class FullBaseTransactionContext<TTransaction, TBlock, TNode>
    : BaseTransactionContext<TTransaction, TBlock, TNode>
    where TTransaction : BaseTransaction<TTransaction, TBlock, TNode>, new()
    where TBlock : BaseBlock<TTransaction, TBlock, TNode>, new()
    where TNode : BaseNode<TTransaction, TBlock, TNode>
{
    public override void CreateTransactions(SimulationContext context)
    {
        var nodes = context.Get<BaseNodes<TTransaction, TBlock, TNode>>();
        var randomness = context.Get<Randomness>();

        var settings = context.Get<Settings>();
        var transactionSettings = settings.Get<TransactionSettings>();
        var simulationSettings = settings.Get<SimulationSettings>();

        var transactionCountDuringSimulation = transactionSettings.RatePerSecond * simulationSettings.LengthInSeconds;

        for (var index = 0; index < transactionCountDuringSimulation; index++)
        {
            var sender = nodes.ElementAt(randomness.Next(nodes.Count));

            var transaction = new TTransaction
            {
                TransactionId = randomness.Next(),
                SenderNode = sender,
                ReceiverNode = nodes.ElementAt(randomness.Next(nodes.Count)),
                TransactionCreateTime = randomness.Next(0, (int)(simulationSettings.LengthInSeconds - 1)),
                SizeInMb = randomness.Expovariate(1 / transactionSettings.AverageSizeInMb),
                Fee = randomness.Expovariate(1 / transactionSettings.AverageFee)
            };

            sender.TransactionPool.Add(transaction);
            PropogateTransaction(context, transaction);
        }
    }

    public override (List<TTransaction>, float) CollectTransactions(SimulationContext context, TNode node,
        float currentTime)
    {
        var settings = context.Get<Settings>();
        var blockSettings = settings.Get<BlockSettings>();

        var transactions = new List<TTransaction>();
        var blockSizeInMb = 0.0f;

        foreach (var transaction in node.TransactionPool
                     .Where(transaction => transaction.TransactionReceiveTime < currentTime)
                     .OrderByDescending(transaction => transaction.Fee))
        {
            if (blockSizeInMb + transaction.SizeInMb >= blockSettings.SizeInMb)
                continue;

            blockSizeInMb += transaction.SizeInMb;
            transactions.Add(transaction);
        }

        return (transactions, blockSizeInMb);
    }

    private void PropogateTransaction(SimulationContext context, TTransaction transaction)
    {
        var nodes = context.Get<BaseNodes<TTransaction, TBlock, TNode>>();
        var network = context.Get<BaseNetwork>();

        foreach (var receiverNode in nodes.Where(node => node.NodeId != transaction.SenderNode.NodeId))
        {
            var clonedTransaction = transaction.Clone();
            clonedTransaction.TransactionReceiveTime =
                transaction.TransactionCreateTime + network.TransactionPropogationDelay(context);
            receiverNode.TransactionPool.Add(clonedTransaction);
        }
    }
}