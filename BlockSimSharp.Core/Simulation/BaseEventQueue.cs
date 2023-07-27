using BlockSimSharp.Core.Model;
using BlockSimSharp.Core.Simulation.TransactionContext;

namespace BlockSimSharp.Core.Simulation;

public abstract class BaseEventQueue<TTransaction, TBlock, TNode, TNetwork, TConsensus, TEvent, TEventQueue, TScheduler, TContext>
    where TTransaction: BaseTransaction<TTransaction>, new()
    where TBlock: BaseBlock<TTransaction, TBlock, TNode>, new()
    where TNode: BaseNode<TTransaction, TBlock, TNode>
    where TNetwork: BaseNetwork<TTransaction, TBlock, TNode, TNetwork, TConsensus, TEvent, TEventQueue, TScheduler, TContext>
    where TConsensus: BaseConsensus<TTransaction, TBlock, TNode, TNetwork, TConsensus, TEvent, TEventQueue, TScheduler, TContext>
    where TScheduler: BaseScheduler<TTransaction, TBlock, TNode, TNetwork, TConsensus, TEvent, TEventQueue, TScheduler, TContext>
    where TEventQueue: BaseEventQueue<TTransaction, TBlock, TNode, TNetwork, TConsensus, TEvent, TEventQueue, TScheduler, TContext>
    where TEvent: BaseEvent<TTransaction, TBlock, TNode, TNetwork, TConsensus, TEvent, TEventQueue, TScheduler, TContext>
    where TContext: BaseContext<TTransaction, TBlock, TNode, TNetwork, TConsensus, TEvent, TEventQueue, TScheduler, TContext>
{
    private readonly PriorityQueue<TEvent, float> _queue;
    
    public BaseEventQueue()
    {
        _queue = new PriorityQueue<TEvent, float>();
    }
    
    public void Enqueue(TEvent baseEvent) => _queue.Enqueue(baseEvent, baseEvent.EventTime);

    public TEvent Dequeue() => _queue.Dequeue();

    public bool IsEmpty() => _queue.Count == 0;
}