using BlockSimSharp.Core.Model;
using BlockSimSharp.Core.Simulation;

namespace BlockSimSharp.Core;

public abstract class BaseScheduler<TTransaction, TBlock, TNode>
    where TTransaction: BaseTransaction<TTransaction>, new()
    where TBlock: BaseBlock<TTransaction, TBlock, TNode>, new()
    where TNode: BaseNode<TTransaction, TBlock, TNode>
{

    public BaseEvent<TTransaction, TBlock, TNode> NextEvent() => _queue.Dequeue();
    public bool HasEvents() => _queue.Count != 0;
    public abstract void ScheduleInitialEvents(TNode node);
    private readonly PriorityQueue<BaseEvent<TTransaction, TBlock, TNode>, float> _queue;
    
    protected BaseScheduler()
    {
        _queue = new PriorityQueue<BaseEvent<TTransaction, TBlock, TNode>, float>();
    }

    protected void Enqueue(BaseEvent<TTransaction, TBlock, TNode> simulationBaseEvent) => _queue.Enqueue(simulationBaseEvent, simulationBaseEvent.EventTime);
    
}