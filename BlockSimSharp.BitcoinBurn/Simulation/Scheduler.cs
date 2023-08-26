using BlockSimSharp.BitcoinBurn.Model;
using BlockSimSharp.BitcoinBurn.Simulation.Events;

namespace BlockSimSharp.BitcoinBurn.Simulation;

public sealed class Scheduler
{
    public Scheduler(
        SimulationEventPool simulationEventPool,
        Randomness randomness, 
        Network network,
        Nodes nodes,
        Consensus consensus)
    {
        _simulationEventPool = simulationEventPool;
        _randomness = randomness;
        _network = network;
        _nodes = nodes;
        _consensus = consensus;
    }

    public void OnBlockMined(SimulationEvent simulationEvent)
    {
        ScheduleReceiveBlockEvents(simulationEvent);
        ScheduleMineBlockEvent(simulationEvent, simulationEvent.Node);
    }

    public void OnBlockReceived(SimulationEvent simulationEvent)
    {
        ScheduleMineBlockEvent(simulationEvent, simulationEvent.Node);
    }

    
    public void ScheduleMineBlockEvent(SimulationEvent? sourceEvent, Node miner)
    {
        if (miner.HashPower <= 0)
            return;

        var scheduledEventTime = sourceEvent?.EventTime ?? 0;
        var eventTime = scheduledEventTime + _consensus.Protocol(miner);

        var block = new Block
        {
            BlockId = _randomness.Next(),
            PreviousBlock = miner.LastBlock,
            Miner = miner,
            Depth = miner.BlockChain.Count,
            ScheduledAt = scheduledEventTime,
            MinedAt = eventTime
        };

        miner.CurrentlyMinedBlock = block;
        
        var @event = new BlockMinedSimulationEvent
        {
            Node = miner,
            EventTime = eventTime,
            Block = block,
        };
        
        _simulationEventPool.Enqueue(@event);
    }
    
    public void ScheduleReceiveBlockEvents(SimulationEvent sourceSimulationEvent)
    {
        foreach (var receiver in _nodes.Without(sourceSimulationEvent.Block.Miner!.NodeId))
        {
            ScheduleReceiveBlockEvent(sourceSimulationEvent, receiver);
        }
    }

    public void ScheduleReceiveBlockEvent(SimulationEvent sourceSimulationEvent, Node receiver)
    {
        var eventTime = sourceSimulationEvent.EventTime + _network.BlockPropogationDelay();
        var @event = new BlockReceivedSimulationEvent
        {
            Node = receiver,
            Block = sourceSimulationEvent.Block,
            EventTime = eventTime,
        };
        
        _simulationEventPool.Enqueue(@event);
    }
    
    private readonly SimulationEventPool _simulationEventPool;
    private readonly Randomness _randomness;
    private readonly Network _network;
    private readonly Nodes _nodes;
    private readonly Consensus _consensus;
}