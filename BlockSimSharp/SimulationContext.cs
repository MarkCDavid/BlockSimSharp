using BlockSimSharp.Base;

namespace BlockSimSharp;

public class SimulationContext<TNode, TBlock>
    where TNode: BaseNode<TBlock>
    where TBlock: BaseBlock, new()
{
    public IReadOnlyList<TNode> Nodes { get; }
    public BaseConsensus<TBlock, TNode> Consensus { get; }
    public Network Network { get; }

    public SimulationContext(IReadOnlyList<TNode> nodes, BaseConsensus<TBlock, TNode> consensus, Network network)
    {
        Nodes = nodes;
        Consensus = consensus;
        Network = network;
    }
}