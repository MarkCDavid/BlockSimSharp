using BlockSimSharp.Bitcoin;

namespace BlockSimSharp.Base;

public abstract class BaseConsensus<TBlock, TNode>
    where TBlock: BaseBlock, new()
    where TNode: BaseNode<TBlock>
{
    protected BaseConsensus()
    {
        GlobalBlockChain = new List<TBlock>();
    }

    public abstract float Protocol(TNode miner);
    public abstract void ForkResolution();

    public List<TBlock> GlobalBlockChain { get; protected set; }
}