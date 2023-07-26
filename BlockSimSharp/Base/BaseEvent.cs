namespace BlockSimSharp.Base;

public abstract class BaseEvent<TNode, TBlock, TTransaction>
    where TNode: BaseNode<TBlock, TTransaction>
    where TBlock: BaseBlock<TTransaction>, new()
    where TTransaction: BaseTransaction<TTransaction>
{
    public TNode Node { get; set; }
    public TBlock Block { get; set; }
    public float EventTime { get; set; }

    public abstract List<BaseEvent<TNode, TBlock, TTransaction>> Handle(SimulationContext<TNode, TBlock, TTransaction> context);
}