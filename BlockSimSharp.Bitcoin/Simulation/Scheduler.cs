using BlockSimSharp.Bitcoin.Model;
using BlockSimSharp.Core.Simulation;

namespace BlockSimSharp.Bitcoin.Simulation;

public class Scheduler: BaseScheduler<Block, Node, Transaction, Scheduler>
{
    public Scheduler(BaseEventQueue<Node, Block, Transaction, Scheduler> eventQueue) : base(eventQueue)
    {
        
    }
    
    // public void TryScheduleMineBlockEvent(
    //     SimulationContext<BitcoinNode, BitcoinBlock, BitcoinTransaction, Scheduler> context, 
    //     BaseEvent<BitcoinNode, BitcoinBlock, BitcoinTransaction, Scheduler> @event,
    //     BitcoinNode miner)
    // {
    //     if (miner.HashPower <= 0)
    //         return;
    //
    //     var eventTime = @event.EventTime + context.Consensus.Protocol(context, miner);
    //
    //     if (eventTime > Configuration.Instance.SimulationLengthInSeconds)
    //         return;
    //
    //     ScheduleMineBlockEvent(miner, eventTime);
    // }
    //
    // public void ScheduleMineBlockEvent(BitcoinNode node, float eventTime)
    // {
    //     EventQueue.Enqueue(new MineBlockEvent
    //     {
    //         Node = node,
    //         EventTime = eventTime,
    //         Block = new BitcoinBlock
    //         {
    //             BlockId = new Random().Next(),
    //             PreviousBlockId = node.LastBlock.BlockId,
    //             MinerId = node.Id,
    //             Depth = node.BlockChain.Count,
    //             Timestamp = eventTime
    //         }
    //     });
    // }
    //
    // public void TryScheduleReceiveBlockEvents(
    //     SimulationContext<BitcoinNode, BitcoinBlock, BitcoinTransaction, Scheduler> context, 
    //     BaseEvent<BitcoinNode, BitcoinBlock, BitcoinTransaction, Scheduler> @event)
    // {
    //     foreach (var receiver in context.Nodes.Where(node => node.Id != @event.Block.MinerId))
    //     {
    //         EventQueue.Enqueue(new ReceiveBlockEvent
    //         {
    //             Node = receiver,
    //             Block = @event.Block,
    //             EventTime = @event.EventTime + context.Network.BlockPropogationDelay(),
    //         });
    //     }
    // }
}