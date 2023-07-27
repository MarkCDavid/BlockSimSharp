using BlockSimSharp.Base;
using BlockSimSharp.Ethereum.Events;

namespace BlockSimSharp.Ethereum;

public class EthereumScheduler: BaseScheduler<EthereumBlock, EthereumNode, EthereumTransaction, EthereumScheduler>
{
    public EthereumScheduler(EventQueue<EthereumNode, EthereumBlock, EthereumTransaction, EthereumScheduler> eventQueue) : base(eventQueue)
    {
        
    }
    
    
    public void TryScheduleMineBlockEvent(
        SimulationContext<EthereumNode, EthereumBlock, EthereumTransaction, EthereumScheduler> context, 
        BaseEvent<EthereumNode, EthereumBlock, EthereumTransaction, EthereumScheduler> @event,
        EthereumNode miner)
    {
        if (miner.HashPower <= 0)
            return;

        var eventTime = @event.EventTime + context.Consensus.Protocol(context, miner);

        if (eventTime > Configuration.Instance.SimulationLengthInSeconds)
            return;

        ScheduleMineBlockEvent(miner, eventTime);
    }

    public void ScheduleMineBlockEvent(EthereumNode node, float eventTime)
    {
        EventQueue.Enqueue(new MineBlockEvent
        {
            Node = node,
            EventTime = eventTime,
            Block = new EthereumBlock
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
        SimulationContext<EthereumNode, EthereumBlock, EthereumTransaction, EthereumScheduler> context, 
        BaseEvent<EthereumNode, EthereumBlock, EthereumTransaction, EthereumScheduler> @event)
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