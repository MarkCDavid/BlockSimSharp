using BlockSimSharp.Core.Model;
using BlockSimSharp.Core.Simulation.TransactionContext;

namespace BlockSimSharp.Core.Simulation;

public abstract class BaseScheduler<TTransaction, TBlock, TNode, TNetwork, TConsensus, TEvent, TEventQueue, TScheduler, TContext>
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
    protected TEventQueue EventQueue { get; init; }
    
    protected BaseScheduler(TEventQueue eventQueue)
    {
        EventQueue = eventQueue;
    }

    public abstract void ScheduleInitialEvents(TNode node);
}