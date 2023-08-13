using BlockSimSharp.Core.Model;

namespace BlockSimSharp.Core.Simulation;

public abstract class BaseScheduler<TTransaction, TBlock, TNode>
    where TTransaction: BaseTransaction<TTransaction, TBlock, TNode>, new()
    where TBlock: BaseBlock<TTransaction, TBlock, TNode>, new()
    where TNode: BaseNode<TTransaction, TBlock, TNode>
{

    public BaseEvent<TTransaction, TBlock, TNode> NextEvent() => _queue.Dequeue();
    public bool HasEvents() => _queue.Count != 0;
    public abstract void ScheduleInitialEvents(SimulationContext context, TNode node);
    private readonly PriorityQueue<BaseEvent<TTransaction, TBlock, TNode>, float> _queue;
    
    protected BaseScheduler()
    {
        _queue = new PriorityQueue<BaseEvent<TTransaction, TBlock, TNode>, float>();
    }

    protected void Enqueue(BaseEvent<TTransaction, TBlock, TNode> simulationBaseEvent) => _queue.Enqueue(simulationBaseEvent, simulationBaseEvent.EventTime);
    
}