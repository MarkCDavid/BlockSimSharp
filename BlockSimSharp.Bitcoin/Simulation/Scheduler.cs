using BlockSimSharp.Bitcoin.Model;
using BlockSimSharp.Bitcoin.Simulation.Events;
using BlockSimSharp.Core.Configuration.Model;
using BlockSimSharp.Core.Simulation;

namespace BlockSimSharp.Bitcoin.Simulation;

public class Scheduler: BaseScheduler<Transaction, Block, Node, Network, Consensus, Event, Scheduler, Context>
{
    
    public override void ScheduleInitialEvents(Node node)
    {
        ScheduleMineBlockEvent(node, 0);
    }
    
    public void TryScheduleMineBlockEvent(Context context, Event @event, Node miner)
    {
        if (miner.HashPower <= 0)
            return;

        var simulationSettings = context.Settings.Get<SimulationSettings>();
        var eventTime = @event.EventTime + context.Consensus.Protocol(context, miner);
    
        if (eventTime > simulationSettings.LengthInSeconds)
            return;
    
        ScheduleMineBlockEvent(miner, eventTime);
    }
    
    public void ScheduleMineBlockEvent(Node node, float eventTime)
    {
        Enqueue(new MineBlockEvent
        {
            Node = node,
            EventTime = eventTime,
            Block = new Block
            {
                BlockId = new Random().Next(),
                PreviousBlockId = node.LastBlock.BlockId,
                MinerId = node.NodeId,
                Depth = node.BlockChain.Count,
                Timestamp = eventTime
            }
        });
    }
    
    public void TryScheduleReceiveBlockEvents(Context context, Event @event)
    {
        foreach (var receiver in context.Nodes.Where(node => node.NodeId != @event.Block.MinerId))
        {
            Enqueue(new ReceiveBlockEvent
            {
                Node = receiver,
                Block = @event.Block,
                EventTime = @event.EventTime + context.Network.BlockPropogationDelay(context),
            });
        }
    }
}