using BlockSimSharp.BitcoinBurn.Model;
using BlockSimSharp.BitcoinBurn.Simulation.Events;
using BlockSimSharp.BitcoinBurn.SimulationStatistics;

namespace BlockSimSharp.BitcoinBurn.Simulation;

public sealed class Scheduler
{
    public Scheduler(
        EventPool eventPool,
        Randomness randomness, 
        Network network,
        Nodes nodes, 
        Difficulty difficulty, 
        Consensus consensus, 
        Statistics statistics)
    {
        _eventPool = eventPool;
        _randomness = randomness;
        _network = network;
        _nodes = nodes;
        _difficulty = difficulty;
        _consensus = consensus;
        _statistics = statistics;
    }

    
    public void ScheduleMineBlockEvent(Event? sourceEvent, Node miner)
    {
        if (miner.HashPower <= 0)
            return;

        var eventTime = sourceEvent?.EventTime ?? 0;
        eventTime += _consensus.Protocol(miner);

        var block = new Block
        {
            BlockId = _randomness.Next(),
            PreviousBlock = miner.LastBlock,
            Miner = miner,
            Depth = miner.BlockChain.Count,
            MinedAt = eventTime
        };

        miner.CurrentlyMinedBlock = block;
        
        var @event = new MineBlockEvent(this, _difficulty, _statistics)
        {
            Node = miner,
            EventTime = eventTime,
            Block = block,
        };
        
        _eventPool.Enqueue(@event);
    }
    
    public void ScheduleReceiveBlockEvents(Event sourceEvent)
    {
        foreach (var receiver in _nodes.Without(sourceEvent.Block.Miner!.NodeId))
        {
            ScheduleReceiveBlockEvent(sourceEvent, receiver);
        }
    }

    public void ScheduleReceiveBlockEvent(Event sourceEvent, Node receiver)
    {
        var eventTime = sourceEvent.EventTime + _network.BlockPropogationDelay();
        var @event = new ReceiveBlockEvent(this)
        {
            Node = receiver,
            Block = sourceEvent.Block,
            EventTime = eventTime,
        };
        
        _eventPool.Enqueue(@event);
    }
    
    private readonly EventPool _eventPool;
    private readonly Randomness _randomness;
    private readonly Network _network;
    private readonly Nodes _nodes;
    private readonly Difficulty _difficulty;
    private readonly Consensus _consensus;
    private readonly Statistics _statistics;
}