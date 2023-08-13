using BlockSimSharp.Bitcoin.Model;
using BlockSimSharp.Bitcoin.Simulation.Events;
using BlockSimSharp.Core;
using BlockSimSharp.Core.Configuration;
using BlockSimSharp.Core.Configuration.Model;
using BlockSimSharp.Core.Simulation;

namespace BlockSimSharp.Bitcoin.Simulation;

public class Scheduler: BaseScheduler<Transaction, Block, Node>
{
    public override void ScheduleInitialEvents(SimulationContext context, Node node)
    {
        ScheduleMineBlockEvent(context, node, 0);
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
    
        ScheduleMineBlockEvent(context, miner, eventTime);
    }
    
    public void ScheduleMineBlockEvent(SimulationContext context, Node node, float eventTime)
    {
        var randomness = context.Get<Randomness>();
        Enqueue(new MineBlockBaseEvent
        {
            Node = node,
            EventTime = eventTime,
            Block = new Block
            {
                BlockId = randomness.Next(),
                PreviousBlock = node.LastBlock,
                Miner = node,
                Depth = node.BlockChain.Count,
                Timestamp = eventTime
            }
        });
    }
    
    public void TryScheduleReceiveBlockEvents(SimulationContext context, BaseEvent<Transaction, Block, Node> baseEvent)
    {
        var nodes = context.Get<Nodes>();
        var network = context.Get<Network>();
        
        foreach (var receiver in nodes.Where(node => node.NodeId != baseEvent.Block.Miner.NodeId))
        {
            Enqueue(new ReceiveBlockBaseEvent
            {
                Node = receiver,
                Block = baseEvent.Block,
                EventTime = baseEvent.EventTime + network.BlockPropogationDelay(context),
            });
        }
    }
}