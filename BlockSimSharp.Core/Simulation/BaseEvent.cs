using BlockSimSharp.Core.Model;

namespace BlockSimSharp.Core.Simulation;

public abstract class BaseEvent<TTransaction, TBlock, TNode, TEvent, TEventQueue, TScheduler, TContext>
    where TTransaction: BaseTransaction<TTransaction>
    where TBlock: BaseBlock<TTransaction>, new()
    where TNode: BaseNode<TTransaction, TBlock, TNode>
    where TScheduler: BaseScheduler<TTransaction, TBlock, TNode, TEvent, TEventQueue, TScheduler, TContext>
    where TEventQueue: BaseEventQueue<TTransaction, TBlock, TNode, TEvent, TEventQueue, TScheduler, TContext>
    where TEvent: BaseEvent<TTransaction, TBlock, TNode, TEvent, TEventQueue, TScheduler, TContext>
    where TContext: BaseContext<TTransaction, TBlock, TNode, TEvent, TEventQueue, TScheduler, TContext>
{
    public TNode Node { get; set; }
    public TBlock Block { get; set; }
    public float EventTime { get; set; }

    public abstract void Handle(TContext context);
}