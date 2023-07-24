using BlockSimSharp.Base;

namespace BlockSimSharp;

public class EventQueue<TNode, TBlock> 
    where TNode: BaseNode<TBlock>
    where TBlock: BaseBlock, new()
{
    private readonly PriorityQueue<BaseEvent<TNode, TBlock>, float> _queue;
    
    public EventQueue()
    {
        _queue = new PriorityQueue<BaseEvent<TNode, TBlock>, float>();
    }
    
    public void Enqueue(BaseEvent<TNode, TBlock> @event) => _queue.Enqueue(@event, @event.EventTime);

    public BaseEvent<TNode, TBlock> Dequeue() => _queue.Dequeue();

    public bool IsEmpty() => _queue.Count == 0;
}