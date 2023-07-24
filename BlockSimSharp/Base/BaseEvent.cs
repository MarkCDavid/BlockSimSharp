namespace BlockSimSharp.Base;

public abstract class BaseEvent<TNode, TBlock>
    where TNode: BaseNode<TBlock>
    where TBlock: BaseBlock, new()
{
    public TNode Node { get; set; }
    public TBlock Block { get; set; }
    public float EventTime { get; set; }

    public abstract List<BaseEvent<TNode, TBlock>> Handle(SimulationContext<TNode, TBlock> context);
}