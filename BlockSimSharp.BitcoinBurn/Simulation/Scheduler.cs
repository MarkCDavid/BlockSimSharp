using BlockSimSharp.BitcoinBurn.Model;
using BlockSimSharp.BitcoinBurn.Simulation.Events;
using BlockSimSharp.BitcoinBurn.SimulationConfiguration;
using BlockSimSharp.BitcoinBurn.SimulationStatistics;
using Node = BlockSimSharp.BitcoinBurn.Model.Node;

namespace BlockSimSharp.BitcoinBurn.Simulation;

public sealed class Scheduler
{
    private readonly Configuration _configuration;
    private readonly Consensus _consensus;
    private readonly IReadOnlyList<Node> _nodes;
    private readonly Network _network;
    private readonly Randomness _randomness;
    private readonly Difficulty _difficulty;
    private readonly Statistics _statistics;

    public Event NextEvent() => _queue.Dequeue();
    public bool HasEvents() => _queue.Count != 0;
    
    private readonly PriorityQueue<Event, double> _queue;
    
    public Scheduler(Configuration configuration, Consensus consensus, IReadOnlyList<Node> nodes, Network network, Randomness randomness, Difficulty difficulty, Statistics statistics)
    {
        _configuration = configuration;
        _consensus = consensus;
        _nodes = nodes;
        _network = network;
        _randomness = randomness;
        _difficulty = difficulty;
        _statistics = statistics;
        _queue = new PriorityQueue<Event, double>();
    }

    private void Enqueue(Event simulationBaseEvent)
    {
        _queue.Enqueue(simulationBaseEvent, simulationBaseEvent.EventTime);
    }

    public void ScheduleInitialEvents(Node node)
    {
        const int eventScheduleTime = 0;
        const int eventTime = 0;
        
        var block = BuildNextBlock(node, eventScheduleTime, eventTime);
        
        ScheduleMineBlockEvent(block, node, 0);
    }
    
    public void TryScheduleMineBlockEvent(Event baseEvent, Node miner)
    {
        if (miner.HashPower <= 0)
            return;

        var eventTime = baseEvent.EventTime + _consensus.Protocol(miner);
    
        var block = BuildNextBlock(miner, baseEvent.EventTime, eventTime);
        
        if (eventTime > _configuration.Simulation.LengthInSeconds)
            return;
    
        ScheduleMineBlockEvent(block, miner, eventTime);
    }
    
    public void TryScheduleReceiveBlockEvents(Event baseEvent)
    {
        
        foreach (var receiver in _nodes.Where(node => node.NodeId != baseEvent.Block.Miner.NodeId))
        {
            Enqueue(new ReceiveBlockEvent(_configuration, this)
            {
                Node = receiver,
                Block = baseEvent.Block,
                EventTime = baseEvent.EventTime + _network.BlockPropogationDelay(),
            });
        }
    }

    private void ScheduleMineBlockEvent(Block block, Node node, double eventTime)
    {
        Enqueue(new MineBlockEvent(_configuration, this, _difficulty, _statistics)
        {
            Node = node,
            EventTime = eventTime,
            Block = block, 
        });
    }

    private Block BuildNextBlock(Node node, double eventScheduleTime, double eventTime)
    {
        var block = new Block
        {
            BlockId = _randomness.Next(),
            PreviousBlock = node.LastBlock,
            Miner = node,
            Depth = node.BlockChain.Count,
            ExecutedAt = eventTime,
            ScheduledAt = eventScheduleTime
        };

        node.CurrentlyMinedBlock = block;
        return block;
    }
}