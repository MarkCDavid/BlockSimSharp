using BlockSimSharp.Core.Model;

namespace BlockSimSharp.Core.Simulation;

public abstract class BaseConsensus<TTransaction, TBlock, TNode, TEvent, TEventQueue, TScheduler, TContext>
    where TTransaction: BaseTransaction<TTransaction>
    where TBlock: BaseBlock<TTransaction>, new()
    where TNode: BaseNode<TTransaction, TBlock, TNode>
    where TScheduler: BaseScheduler<TTransaction, TBlock, TNode, TEvent, TEventQueue, TScheduler, TContext>
    where TEventQueue: BaseEventQueue<TTransaction, TBlock, TNode, TEvent, TEventQueue, TScheduler, TContext>
    where TEvent: BaseEvent<TTransaction, TBlock, TNode, TEvent, TEventQueue, TScheduler, TContext>
    where TContext: BaseContext<TTransaction, TBlock, TNode, TEvent, TEventQueue, TScheduler, TContext>
{
    protected BaseConsensus()
    {
        GlobalBlockChain = new List<TBlock>();
    }

    public abstract float Protocol(TContext baseContext, TNode miner);
    public abstract void ForkResolution(TContext baseContext);

    public List<TBlock> GlobalBlockChain { get; protected set; }
}