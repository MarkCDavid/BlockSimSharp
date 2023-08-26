using BlockSimSharp.BitcoinBurn.Simulation.Events;

namespace BlockSimSharp.BitcoinBurn.Simulation;

public class PowerUsage
{
    public void OnEvent(SimulationEvent simulationEvent)
    {
        var timeSpentMining = simulationEvent.EventTime - simulationEvent.Node.CurrentlyMinedBlock!.ScheduledAt;
        simulationEvent.Node.PowerUsed += timeSpentMining * simulationEvent.Node.HashPower;
    }
}