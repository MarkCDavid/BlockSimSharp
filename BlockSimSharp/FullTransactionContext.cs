using BlockSimSharp.Base;

namespace BlockSimSharp;

public class FullTransactionContext<TNode, TBlock, TTransaction, TScheduler>: ITransactionContext<TNode, TBlock, TTransaction, TScheduler>
    where TNode: BaseNode<TBlock, TTransaction>
    where TBlock: BaseBlock<TTransaction>, new()
    where TTransaction: BaseTransaction<TTransaction>, new()
    where TScheduler: BaseScheduler<TBlock, TNode, TTransaction, TScheduler>
{

    public void CreateTransactions(SimulationContext<TNode, TBlock, TTransaction, TScheduler> context)
    {
        var random = new Random();
        var transactionCountDuringSimulation = Configuration.Instance.TransactionsPerSecond * Configuration.Instance.SimulationLengthInSeconds;

        for (var index = 0; index < transactionCountDuringSimulation; index++)
        {
            var sender = context.Nodes.ElementAt(random.Next(context.Nodes.Count));

            var transaction = new TTransaction()
            {
                TransactionId = random.Next(),
                SenderNodeId = sender.Id,
                ReceiverNodeId = context.Nodes.ElementAt(random.Next(context.Nodes.Count)).Id,
                TransactionCreateTime = random.Next(0, Configuration.Instance.SimulationLengthInSeconds - 1),
                SizeInMb = Utility.Expovariate(1 / Configuration.Instance.AverageTransactionSizeInMb),
                Fee = Utility.Expovariate(1 / Configuration.Instance.AverageTransactionFee)
            };
            
            sender.TransactionPool.Add(transaction);
            PropogateTransaction(context, transaction);
        }
    }

    public (List<TTransaction>, float) CollectTransactions(TNode node, float currentTime)
    {
        var transactions = new List<TTransaction>();
        var blockSizeInMb = 0.0f;

        foreach (var transaction in node.TransactionPool.Where(transaction => transaction.TransactionReceiveTime < currentTime).OrderByDescending(transaction => transaction.Fee))
        {
            if (blockSizeInMb + transaction.SizeInMb >= Configuration.Instance.BlockSizeInMb) 
                continue;
            
            blockSizeInMb += transaction.SizeInMb;
            transactions.Add(transaction);
        }

        return (transactions, blockSizeInMb);
    }

    private void PropogateTransaction(SimulationContext<TNode, TBlock, TTransaction, TScheduler> context, TTransaction transaction)
    {
        foreach (var receiverNode in context.Nodes.Where(node => node.Id != transaction.SenderNodeId))
        {
            var clonedTransaction = transaction.DeepClone();
            clonedTransaction.TransactionReceiveTime = transaction.TransactionCreateTime + context.Network.TransactionPropogationDelay();
            receiverNode.TransactionPool.Add(clonedTransaction);
        }
    }
}