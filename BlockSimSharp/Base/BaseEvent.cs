namespace BlockSimSharp.Base;

public abstract class BaseEvent<TNode, TBlock, TTransaction, TScheduler>
    where TNode: BaseNode<TBlock, TTransaction>
    where TBlock: BaseBlock<TTransaction>, new()
    where TTransaction: BaseTransaction<TTransaction>
    where TScheduler: BaseScheduler<TBlock, TNode, TTransaction, TScheduler>
{
    public TNode Node { get; set; }
    public TBlock Block { get; set; }
    public float EventTime { get; set; }

    public abstract void Handle(SimulationContext<TNode, TBlock, TTransaction, TScheduler> context);
}