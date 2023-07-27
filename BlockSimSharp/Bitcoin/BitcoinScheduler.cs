using BlockSimSharp.Base;
using BlockSimSharp.Bitcoin.Events;

namespace BlockSimSharp.Bitcoin;

public class BitcoinScheduler: BaseScheduler<BitcoinBlock, BitcoinNode, BitcoinTransaction, BitcoinScheduler>
{
    public BitcoinScheduler(EventQueue<BitcoinNode, BitcoinBlock, BitcoinTransaction, BitcoinScheduler> eventQueue) : base(eventQueue)
    {
        
    }
    
    public void TryScheduleMineBlockEvent(
        SimulationContext<BitcoinNode, BitcoinBlock, BitcoinTransaction, BitcoinScheduler> context, 
        BaseEvent<BitcoinNode, BitcoinBlock, BitcoinTransaction, BitcoinScheduler> @event,
        BitcoinNode miner)
    {
        if (miner.HashPower <= 0)
            return;

        var eventTime = @event.EventTime + context.Consensus.Protocol(context, miner);

        if (eventTime > Configuration.Instance.SimulationLengthInSeconds)
            return;

        ScheduleMineBlockEvent(miner, eventTime);
    }

    public void ScheduleMineBlockEvent(BitcoinNode node, float eventTime)
    {
        EventQueue.Enqueue(new MineBlockEvent
        {
            Node = node,
            EventTime = eventTime,
            Block = new BitcoinBlock
            {
                BlockId = new Random().Next(),
                PreviousBlockId = node.LastBlock.BlockId,
                MinerId = node.Id,
                Depth = node.BlockChain.Count,
                Timestamp = eventTime
            }
        });
    }

    public void TryScheduleReceiveBlockEvents(
        SimulationContext<BitcoinNode, BitcoinBlock, BitcoinTransaction, BitcoinScheduler> context, 
        BaseEvent<BitcoinNode, BitcoinBlock, BitcoinTransaction, BitcoinScheduler> @event)
    {
        foreach (var receiver in context.Nodes.Where(node => node.Id != @event.Block.MinerId))
        {
            EventQueue.Enqueue(new ReceiveBlockEvent
            {
                Node = receiver,
                Block = @event.Block,
                EventTime = @event.EventTime + context.Network.BlockPropogationDelay(),
            });
        }
    }
}