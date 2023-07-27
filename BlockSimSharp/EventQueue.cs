using BlockSimSharp.Base;

namespace BlockSimSharp;

public class EventQueue<TNode, TBlock, TTransaction, TScheduler> 
    where TNode: BaseNode<TBlock, TTransaction>
    where TBlock: BaseBlock<TTransaction>, new()
    where TTransaction: BaseTransaction<TTransaction>
    where TScheduler: BaseScheduler<TBlock, TNode, TTransaction, TScheduler>
{
    private readonly PriorityQueue<BaseEvent<TNode, TBlock, TTransaction, TScheduler>, float> _queue;
    
    public EventQueue()
    {
        _queue = new PriorityQueue<BaseEvent<TNode, TBlock, TTransaction, TScheduler>, float>();
    }
    
    public void Enqueue(BaseEvent<TNode, TBlock, TTransaction, TScheduler> @event) => _queue.Enqueue(@event, @event.EventTime);

    public BaseEvent<TNode, TBlock, TTransaction, TScheduler> Dequeue() => _queue.Dequeue();

    public bool IsEmpty() => _queue.Count == 0;
}