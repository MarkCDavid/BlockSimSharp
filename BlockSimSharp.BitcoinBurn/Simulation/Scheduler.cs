using BlockSimSharp.BitcoinBurn.Model;
using BlockSimSharp.BitcoinBurn.Simulation.Events;
using BlockSimSharp.Core;
using BlockSimSharp.Core.Configuration;
using BlockSimSharp.Core.Configuration.Model;
using BlockSimSharp.Core.Simulation;

namespace BlockSimSharp.BitcoinBurn.Simulation;

public class Scheduler: BaseScheduler<Transaction, Block, Node>
{
    public override void ScheduleInitialEvents(Node node)
    {
        ScheduleMineBlockEvent(node, 0, 0);
    }
    
    public void TryScheduleMineBlockEvent(SimulationContext context, BaseEvent<Transaction, Block, Node> baseEvent, Node miner)
    {
        var settings = context.Get<Settings>();
        var consensus = context.Get<Consensus>();
        
        if (miner.HashPower <= 0)
            return;

        var simulationSettings = settings.Get<SimulationSettings>();
        var eventTime = baseEvent.EventTime + consensus.Protocol(context, miner);
    
        if (eventTime > simulationSettings.LengthInSeconds)
            return;
    
        ScheduleMineBlockEvent(miner, baseEvent.EventTime, eventTime);
    }
    
    public void ScheduleMineBlockEvent(Node node, float eventScheduleTime, float eventTime)
    {
        var block = new Block
        {
            BlockId = new Random().Next(),
            PreviousBlock = node.LastBlock,
            Miner = node,
            Depth = node.BlockChain.Count,
            Timestamp = eventTime,
            ScheduledTimestamp = eventScheduleTime
        };

        node.CurrentlyMinedBlock = block;
        
        Enqueue(new MineBlockEvent
        {
            Node = node,
            EventTime = eventTime,
            Block = block, 
        });
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
}