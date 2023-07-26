using BlockSimSharp.Base;

namespace BlockSimSharp;

public class EventQueue<TNode, TBlock, TTransaction> 
    where TNode: BaseNode<TBlock, TTransaction>
    where TBlock: BaseBlock<TTransaction>, new()
    where TTransaction: BaseTransaction<TTransaction>
{
    private readonly PriorityQueue<BaseEvent<TNode, TBlock, TTransaction>, float> _queue;
    
    public EventQueue()
    {
        _queue = new PriorityQueue<BaseEvent<TNode, TBlock, TTransaction>, float>();
    }
    
    public void Enqueue(BaseEvent<TNode, TBlock, TTransaction> @event) => _queue.Enqueue(@event, @event.EventTime);

    public BaseEvent<TNode, TBlock, TTransaction> Dequeue() => _queue.Dequeue();

    public bool IsEmpty() => _queue.Count == 0;
}