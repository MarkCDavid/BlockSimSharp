using BlockSimSharp.Core.Model;

namespace BlockSimSharp.Core.Simulation;

public abstract class BaseEvent<TTransaction, TBlock, TNode>
    where TTransaction: BaseTransaction<TTransaction, TBlock, TNode>, new()
    where TBlock: BaseBlock<TTransaction, TBlock, TNode>, new()
    where TNode: BaseNode<TTransaction, TBlock, TNode>
{
    public TNode Node { get; init; } = null!;
    public TBlock Block { get; init; } = null!;
    public float EventTime { get; init; }

    public abstract void Handle(SimulationContext context);
}