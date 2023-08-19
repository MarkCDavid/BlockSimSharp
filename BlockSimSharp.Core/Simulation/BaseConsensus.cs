using BlockSimSharp.Core.Model;

namespace BlockSimSharp.Core.Simulation;

public abstract class BaseConsensus<TTransaction, TBlock, TNode>
    where TTransaction: BaseTransaction<TTransaction, TBlock, TNode>, new()
    where TBlock: BaseBlock<TTransaction, TBlock, TNode>, new()
    where TNode: BaseNode<TTransaction, TBlock, TNode>
{
    
    public abstract float Protocol(SimulationContext baseContext, TNode miner);
    
    public abstract void ResolveForks(SimulationContext baseContext);

    
}