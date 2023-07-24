using BlockSimSharp.Base;

namespace BlockSimSharp;

public class SimulationContext<TNode, TBlock, TTransaction>
    where TNode: BaseNode<TBlock, TTransaction>
    where TBlock: BaseBlock<TTransaction>, new()
    where TTransaction: BaseTransaction
{
    public IReadOnlyList<TNode> Nodes { get; }
    public BaseConsensus<TBlock, TNode, TTransaction> Consensus { get; }
    public Network Network { get; }
    public ITransactionContext<TNode, TBlock, TTransaction> TransactionContext { get; }

    public SimulationContext(IReadOnlyList<TNode> nodes, BaseConsensus<TBlock, TNode, TTransaction> consensus, Network network, ITransactionContext<TNode, TBlock, TTransaction> transactionContext)
    {
        Nodes = nodes;
        Consensus = consensus;
        Network = network;
        TransactionContext = transactionContext;
    }
}