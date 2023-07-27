using BlockSimSharp.Bitcoin;

namespace BlockSimSharp.Base;

public abstract class BaseConsensus<TBlock, TNode, TTransaction>
    where TBlock: BaseBlock<TTransaction>, new()
    where TNode: BaseNode<TBlock, TTransaction>
    where TTransaction: BaseTransaction<TTransaction>
{
    protected BaseConsensus()
    {
        GlobalBlockChain = new List<TBlock>();
    }

    public abstract float Protocol(SimulationContext<TNode, TBlock, TTransaction> context, TNode miner);
    public abstract void ForkResolution(SimulationContext<TNode, TBlock, TTransaction> context);

    public List<TBlock> GlobalBlockChain { get; protected set; }
}