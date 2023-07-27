namespace BlockSimSharp.Base;

public abstract class BaseScheduler<TBlock, TNode, TTransaction, TScheduler>
    where TNode: BaseNode<TBlock, TTransaction>
    where TBlock: BaseBlock<TTransaction>, new()
    where TTransaction: BaseTransaction<TTransaction>
    where TScheduler: BaseScheduler<TBlock, TNode, TTransaction, TScheduler>
{
    protected EventQueue<TNode, TBlock, TTransaction, TScheduler> EventQueue { get; init; }

    protected BaseScheduler(EventQueue<TNode, TBlock, TTransaction, TScheduler> eventQueue)
    {
        EventQueue = eventQueue;
    }
}