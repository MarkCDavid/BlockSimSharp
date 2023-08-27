using BlockSimSharp.BitcoinBurn.Model;
using BlockSimSharp.BitcoinBurn.Simulation.Events;
using BlockSimSharp.BitcoinBurn.Utility;

namespace BlockSimSharp.BitcoinBurn.Simulation.Data;

public class PowerUsage
{
    private readonly DefaultDictionary<Node, double> _usedPower = new(0.0);

    public void OnBlockMined(BlockMinedSimulationEvent simulationEvent)
    {
        AccumulateUsedPower(simulationEvent.Miner, simulationEvent.EventTime);
    }

    public void OnBlockReceived(BlockReceivedSimulationEvent simulationEvent)
    {
        AccumulateUsedPower(simulationEvent.Recipient, simulationEvent.EventTime);
    }

    public double PowerUsed(Node node) => _usedPower[node];
    public double TotalPowerUsed() => _usedPower.Values.Sum();
    
    private void AccumulateUsedPower(Node miner, double eventTime) 
    {
        var timeSpentMining = eventTime - miner.CurrentlyMinedBlock!.ScheduledAt;
        _usedPower[miner] += timeSpentMining * miner.HashPower;
    }
}