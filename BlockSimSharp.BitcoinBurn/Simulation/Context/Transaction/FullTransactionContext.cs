using BlockSimSharp.BitcoinBurn.Model;
using BlockSimSharp.BitcoinBurn.SimulationConfiguration;

namespace BlockSimSharp.BitcoinBurn.Simulation.Context.Transaction;

public sealed class FullTransactionContext: TransactionContext
{
    private readonly Configuration _configuration;
    private readonly Randomness _randomness;
    private readonly IReadOnlyList<Node> _nodes;
    private readonly Network _network;

    public FullTransactionContext(Configuration configuration, Randomness randomness, IReadOnlyList<Node> nodes, Network network)
    {
        _configuration = configuration;
        _randomness = randomness;
        _nodes = nodes;
        _network = network;
    }

    public override void CreateTransactions()
    {
        var transactionCountDuringSimulation = _configuration.Transaction.RatePerSecond * _configuration.Simulation.LengthInSeconds;

        for (var index = 0; index < transactionCountDuringSimulation; index++)
        {
            var sender = _nodes.ElementAt(_randomness.Next(_nodes.Count));

            var transaction = new Model.Transaction
            {
                TransactionId = _randomness.Next(),
                SenderNode = sender,
                ReceiverNode = _nodes.ElementAt(_randomness.Next(_nodes.Count)),
                TransactionCreateTime = _randomness.Next(0, _configuration.Simulation.LengthInSeconds - 1),
                SizeInMb = _randomness.Expovariate(1 / _configuration.Transaction.AverageSizeInMb),
                Fee = _randomness.Expovariate(1 / _configuration.Transaction.AverageFee)
            };

            sender.TransactionPool.Add(transaction);
            PropogateTransaction(transaction);
        }
    }

    public override (List<Model.Transaction>, double) CollectTransactions(Node node, double currentTime)
    {
        var transactions = new List<Model.Transaction>();
        var blockSizeInMb = 0.0;

        foreach (var transaction in node.TransactionPool
                     .Where(transaction => transaction.TransactionReceiveTime < currentTime)
                     .OrderByDescending(transaction => transaction.Fee))
        {
            if (blockSizeInMb + transaction.SizeInMb >= _configuration.Block.SizeInMb)
                continue;

            blockSizeInMb += transaction.SizeInMb;
            transactions.Add(transaction);
        }

        return (transactions, blockSizeInMb);
    }

    private void PropogateTransaction(Model.Transaction transaction)
    {
        foreach (var receiverNode in _nodes.Where(node => node.NodeId != transaction.SenderNode.NodeId))
        {
            var clonedTransaction = transaction.Clone();
            clonedTransaction.TransactionReceiveTime =
                transaction.TransactionCreateTime + _network.TransactionPropogationDelay();
            receiverNode.TransactionPool.Add(clonedTransaction);
        }
    }
}