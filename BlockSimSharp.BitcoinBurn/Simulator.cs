using BlockSimSharp.BitcoinBurn.Simulation.Events;
using BlockSimSharp.BitcoinBurn.SimulationConfiguration;
using BlockSimSharp.BitcoinBurn.Utility;

namespace BlockSimSharp.BitcoinBurn;

public class Simulator
{
    public IntegrationEvent SimulationStartingIntegrationEvent { get; } = new();
    public IntegrationEvent<BlockMinedSimulationEvent> BlockMinedIntegrationEvent { get; } = new();
    public IntegrationEvent<BlockReceivedSimulationEvent> BlockReceivedIntegrationEvent { get; } = new();
    public IntegrationEvent SimulationStoppingIntegrationEvent { get; } = new();

    public Simulator(SimulationEventPool simulationEventPool)
    {
        _simulationEventPool = simulationEventPool;
    }

    public void Simulate()
    {
        SimulationStartingIntegrationEvent.Invoke();
        
        RunSimulation();
        
        SimulationStoppingIntegrationEvent.Invoke();
    }

    private void RunSimulation()
    {
        while (_simulationEventPool.HasEvents())
        {
            _simulationEventPool.NextEvent().Handle(this);
        }
    }
    
    private readonly SimulationEventPool _simulationEventPool;
}