using BlockSimSharp.BitcoinBurn.SimulationConfiguration;

namespace BlockSimSharp.BitcoinBurn.Simulation.Events;

public class EventPool
{
    public Event NextEvent() => _queue.Dequeue();
    public bool HasEvents() => _queue.Count != 0;
    
    public EventPool(Configuration configuration)
    {
        _configuration = configuration;
        _queue = new PriorityQueue<Event, double>();
    }

    public void Enqueue(Event @event)
    {
        if (@event.EventTime < _configuration.Simulation.LengthInSeconds)
        {
            _queue.Enqueue(@event, @event.EventTime);
        }
    }

    private readonly Configuration _configuration;
    private readonly PriorityQueue<Event, double> _queue;
}