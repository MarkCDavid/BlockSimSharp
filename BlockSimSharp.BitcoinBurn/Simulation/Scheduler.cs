using BlockSimSharp.BitcoinBurn.Model;
using BlockSimSharp.BitcoinBurn.Simulation.Events;
using BlockSimSharp.Core;
using BlockSimSharp.Core.Configuration;
using BlockSimSharp.Core.Configuration.Model;
using BlockSimSharp.Core.Simulation;

namespace BlockSimSharp.BitcoinBurn.Simulation;

public class Scheduler: BaseScheduler<Transaction, Block, Node>
{
    public override void ScheduleInitialEvents(SimulationContext context, Node node)
    {
        const int eventScheduleTime = 0;
        const int eventTime = 0;
        
        var block = BuildNextBlock(context, node, eventScheduleTime, eventTime);
        
        ScheduleMineBlockEvent(block, node, 0);
    }
    
    public void TryScheduleMineBlockEvent(SimulationContext context, BaseEvent<Transaction, Block, Node> baseEvent, Node miner)
    {
        var settings = context.Get<Settings>();
        var consensus = context.Get<Consensus>();
        
        if (miner.HashPower <= 0)
            return;

        var simulationSettings = settings.Get<SimulationSettings>();
        var eventTime = baseEvent.EventTime + consensus.Protocol(context, miner);
    
        var block = BuildNextBlock(context, miner, baseEvent.EventTime, eventTime);
        
        if (eventTime > simulationSettings.LengthInSeconds)
            return;
    
        ScheduleMineBlockEvent(block, miner, eventTime);
    }
    
    public void TryScheduleReceiveBlockEvents(SimulationContext context, BaseEvent<Transaction, Block, Node> baseEvent)
    {
        var nodes = context.Get<Nodes>();
        var network = context.Get<Network>();
        
        foreach (var receiver in nodes.Where(node => node.NodeId != baseEvent.Block.Miner.NodeId))
        {
            Enqueue(new ReceiveBlockEvent
            {
                Node = receiver,
                Block = baseEvent.Block,
                EventTime = baseEvent.EventTime + network.BlockPropogationDelay(context),
            });
        }
    }

    private void ScheduleMineBlockEvent(Block block, Node node, float eventTime)
    {
        Enqueue(new MineBlockEvent
        {
            Node = node,
            EventTime = eventTime,
            Block = block, 
        });
    }

    private static Block BuildNextBlock(SimulationContext context, Node node, float eventScheduleTime, float eventTime)
    {
        var randomness = context.Get<Randomness>();
        var block = new Block
        {
            BlockId = randomness.Next(),
            PreviousBlock = node.LastBlock,
            Miner = node,
            Depth = node.BlockChain.Count,
            Timestamp = eventTime,
            ScheduledTimestamp = eventScheduleTime
        };

        node.CurrentlyMinedBlock = block;
        return block;
    }
}