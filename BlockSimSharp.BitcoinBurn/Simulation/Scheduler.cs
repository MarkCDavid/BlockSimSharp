using BlockSimSharp.BitcoinBurn.Model;
using BlockSimSharp.BitcoinBurn.Simulation.Events;

namespace BlockSimSharp.BitcoinBurn.Simulation;

public sealed class Scheduler
{
    public Scheduler(
        SimulationEventPool simulationEventPool,
        EventTimeDeltas eventTimeDeltas,
        Randomness randomness,
        Nodes nodes)
    {
        _simulationEventPool = simulationEventPool;
        _eventTimeDeltas = eventTimeDeltas;
        _randomness = randomness;
        _nodes = nodes;
    }

    public void ScheduleInitialEvents()
    {
        foreach (var node in _nodes)
        {
            var simulationEvent = new BlockMinedSimulationEvent()
            {
                Node = node,
                EventTime = 0
            };
            
            ScheduleMineBlockEvent(simulationEvent);
        }
    }

    public void OnBlockMined(BlockMinedSimulationEvent simulationEvent)
    {
        ScheduleReceiveBlockEvents(simulationEvent);
        ScheduleMineBlockEvent(simulationEvent);
    }

    public void OnBlockReceived(BlockReceivedSimulationEvent simulationEvent)
    {
        // Given that we were mining a block when we received another block,
        // we could have accepted that block (or the entire chain), which
        // would have changed the block that our currently mined block is
        // based on. If that is the case, we need to start mining a new
        // block. If not - we continue mining the previous block.
        var recipient = simulationEvent.Recipient;
        var minedBlock = recipient.CurrentlyMinedBlock;
        if (recipient.LastBlock.Equals(minedBlock?.PreviousBlock))
        {
            return;
        }
        
        ScheduleMineBlockEvent(simulationEvent);
    }


    private void ScheduleMineBlockEvent(SimulationEvent sourceEvent)
    {
        var miner = sourceEvent.Node;
        
        if (miner.HashPower <= 0)
            return;

        var eventScheduleTime = sourceEvent.EventTime;
        var eventTime = eventScheduleTime + _eventTimeDeltas.BlockMiningTimeDelta(miner);

        var block = new Block
        {
            BlockId = _randomness.Next(),
            PreviousBlock = miner.LastBlock,
            Miner = miner,
            Depth = miner.BlockChain.Count,
            ScheduledAt = eventScheduleTime,
            MinedAt = eventTime
        };

        miner.CurrentlyMinedBlock = block;
        
        var simulationEvent = new BlockMinedSimulationEvent
        {
            Node = miner,
            EventTime = eventTime,
            Block = block,
        };
        
        _simulationEventPool.Enqueue(simulationEvent);
    }

    private void ScheduleReceiveBlockEvents(SimulationEvent simulationEvent)
    {
        foreach (var receiver in _nodes.Without(simulationEvent.Block.Miner!.NodeId))
        {
            ScheduleReceiveBlockEvent(simulationEvent, receiver);
        }
    }

    private void ScheduleReceiveBlockEvent(SimulationEvent sourceSimulationEvent, Node receiver)
    {
        var eventTime = sourceSimulationEvent.EventTime + _eventTimeDeltas.BlockPropogationTimeDelta();
        
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
    private readonly Nodes _nodes;
    private readonly EventTimeDeltas _eventTimeDeltas;
}