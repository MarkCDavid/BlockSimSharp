using BlockSimSharp.Base;

namespace BlockSimSharp;

public class SimulationContext<TNode, TBlock, TTransaction, TScheduler>
    where TNode: BaseNode<TBlock, TTransaction>
    where TBlock: BaseBlock<TTransaction>, new()
    where TTransaction: BaseTransaction<TTransaction>
    where TScheduler: BaseScheduler<TBlock, TNode, TTransaction, TScheduler>
{
    public IReadOnlyList<TNode> Nodes { get; }
    public BaseConsensus<TBlock, TNode, TTransaction, TScheduler> Consensus { get; }
    public Network Network { get; }
    public TScheduler Scheduler { get; }
    public ITransactionContext<TNode, TBlock, TTransaction, TScheduler> TransactionContext { get; }
    public Statistics.Statistics<TNode, TBlock, TTransaction, TScheduler> Statistics { get; }

    public SimulationContext(
        IReadOnlyList<TNode> nodes, 
        BaseConsensus<TBlock, TNode, TTransaction, TScheduler> consensus, 
        Network network, 
        TScheduler scheduler,
        ITransactionContext<TNode, TBlock, TTransaction, TScheduler> transactionContext, 
        Statistics.Statistics<TNode, TBlock, TTransaction, TScheduler> statistics)
    {
        Nodes = nodes;
        Consensus = consensus;
        Network = network;
        Scheduler = scheduler;
        TransactionContext = transactionContext;
        Statistics = statistics;
    }
}