using BlockSimSharp.BitcoinBurn.SimulationConfiguration;

namespace BlockSimSharp.BitcoinBurn.Simulation.Events;

public class SimulationEventPool
{
    public SimulationEvent NextEvent() => _queue.Dequeue();
    public bool HasEvents() => _queue.Count != 0;
    
    public SimulationEventPool(Configuration configuration)
    {
        _configuration = configuration;
        _queue = new PriorityQueue<SimulationEvent, double>();
    }

    public void Enqueue(SimulationEvent simulationEvent)
    {
        if (simulationEvent.EventTime < _configuration.Simulation.LengthInSeconds)
        {
            _queue.Enqueue(simulationEvent, simulationEvent.EventTime);
        }
    }

    private readonly Configuration _configuration;
    private readonly PriorityQueue<SimulationEvent, double> _queue;
}