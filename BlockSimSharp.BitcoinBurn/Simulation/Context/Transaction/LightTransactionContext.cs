using BlockSimSharp.BitcoinBurn.Model;
using BlockSimSharp.BitcoinBurn.SimulationConfiguration;

namespace BlockSimSharp.BitcoinBurn.Simulation.Context.Transaction;

public sealed class LightTransactionContext: TransactionContext
{
    private readonly Configuration _configuration;
    private readonly Randomness _randomness;
    private readonly IReadOnlyList<Node> _nodes;
    private List<Model.Transaction> LocalTransactionPool { get; set; }
    public LightTransactionContext(Configuration configuration, Randomness randomness, IReadOnlyList<Node> nodes)
    {
        _configuration = configuration;
        _randomness = randomness;
        _nodes = nodes;
        LocalTransactionPool = new List<Model.Transaction>();
    }

    public override void CreateTransactions()
    {
        var transactionCountInBlock 
            = _configuration.Transaction.RatePerSecond * _configuration.Block.AverageIntervalInSeconds;
        
        LocalTransactionPool = Enumerable.Range(0, transactionCountInBlock)
            .Select(_ => new Model.Transaction()
            {
                TransactionId = _randomness.Next(),
                SenderNode = _nodes.ElementAt(_randomness.Next(_nodes.Count)),
                ReceiverNode = _nodes.ElementAt(_randomness.Next(_nodes.Count)),
                SizeInMb = _randomness.Expovariate(1 / _configuration.Transaction.AverageSizeInMb),
                Fee = _randomness.Expovariate(1 / _configuration.Transaction.AverageFee)
            }).ToList();
    }

    public override (List<Model.Transaction>, double) CollectTransactions(Node node, double currentTime)
    {
        var transactions = new List<Model.Transaction>();
        var blockSizeInMb = 0.0;
        
        foreach (var transaction in LocalTransactionPool.OrderByDescending(transaction => transaction.Fee))
        {
            if (blockSizeInMb + transaction.SizeInMb >= _configuration.Block.SizeInMb) 
                continue;
            
            blockSizeInMb += transaction.SizeInMb;
            transactions.Add(transaction);
        }
        
        return (transactions, blockSizeInMb);
    }
}