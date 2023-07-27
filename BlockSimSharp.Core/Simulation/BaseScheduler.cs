using BlockSimSharp.Core.Model;
using BlockSimSharp.Core.Simulation.TransactionContext;

namespace BlockSimSharp.Core.Simulation;

public abstract class BaseScheduler<TTransaction, TBlock, TNode, TNetwork, TConsensus, TEvent, TScheduler, TContext>
    where TTransaction: BaseTransaction<TTransaction>, new()
    where TBlock: BaseBlock<TTransaction, TBlock, TNode>, new()
    where TNode: BaseNode<TTransaction, TBlock, TNode>
    where TNetwork: BaseNetwork<TTransaction, TBlock, TNode, TNetwork, TConsensus, TEvent, TScheduler, TContext>
    where TConsensus: BaseConsensus<TTransaction, TBlock, TNode, TNetwork, TConsensus, TEvent, TScheduler, TContext>
    where TScheduler: BaseScheduler<TTransaction, TBlock, TNode, TNetwork, TConsensus, TEvent, TScheduler, TContext>
    where TEvent: BaseEvent<TTransaction, TBlock, TNode, TNetwork, TConsensus, TEvent, TScheduler, TContext>
    where TContext: BaseContext<TTransaction, TBlock, TNode, TNetwork, TConsensus, TEvent, TScheduler, TContext>
{

    public TEvent NextEvent() => _queue.Dequeue();
    public bool HasEvents() => _queue.Count != 0;
    public abstract void ScheduleInitialEvents(TNode node);
    
    private readonly PriorityQueue<TEvent, float> _queue;
    protected BaseScheduler()
    {
        _queue = new PriorityQueue<TEvent, float>();
    }

    protected void Enqueue(TEvent baseEvent) => _queue.Enqueue(baseEvent, baseEvent.EventTime);
    
}