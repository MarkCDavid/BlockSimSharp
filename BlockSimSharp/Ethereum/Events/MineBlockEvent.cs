using BlockSimSharp.Base;

namespace BlockSimSharp.Ethereum.Events;

public class MineBlockEvent: BaseEvent<EthereumNode, EthereumBlock, EthereumTransaction>
{
    public MineBlockEvent(EthereumNode node, float eventTime)
    {
        Node = node;
        EventTime = eventTime;
        Block = new EthereumBlock
        {
            BlockId = new Random().Next(),
            PreviousBlockId = node.LastBlock.BlockId,
            MinerId = node.Id,
            Depth = node.BlockChain.Count,
            Timestamp = eventTime
        };
    }
    public override List<BaseEvent<EthereumNode, EthereumBlock, EthereumTransaction>> Handle(SimulationContext<EthereumNode, EthereumBlock, EthereumTransaction> context)
    {
        var futureEvents = new List<BaseEvent<EthereumNode, EthereumBlock, EthereumTransaction>>();
        
        var miner = context.Nodes.FirstOrDefault(node => node.Id == Node.Id);

        if (miner is null)
            throw new Exception($"Node with Id {Node.Id} does not exist!");

        // This check allows us to discard an event that has already been scheduled,
        // because this node has accepted a new block into the blockchain in the
        // meantime.
        var blockSequenceValid = Block.PreviousBlockId == miner.LastBlock.BlockId;
        if (!blockSequenceValid)
            return futureEvents;
        
        context.Statistics.TotalBlocks += 1;
        
        if (Configuration.Instance.TransactionsEnabled)
        {
            var (transactions, sizeInMb) = context.TransactionContext.CollectTransactions(miner, EventTime);
            Block.Transactions = transactions;
            Block.UsedGas = sizeInMb;
        }
        
        if (Configuration.Instance.UnclesEnabled)
        {
            miner.UpdateUncleChain();
            Block.Uncles = miner.CollectUncles();
        }
        
        miner.BlockChain.Add(Block);
        
        if (Configuration.Instance.TransactionsEnabled && Configuration.Instance.TransactionContextType == "light")
        {
            context.TransactionContext.CreateTransactions(context);
        }

        var receiveBlockEvents = GenerateReceiveBlockEvents(context);
        if (receiveBlockEvents.Any())
            futureEvents.AddRange(receiveBlockEvents);

        var mineEvent = GenerateMineBlockEvent(context, miner);
        if (mineEvent is not null)
            futureEvents.Add(mineEvent);

        return futureEvents;
    }
    
    private List<ReceiveBlockEvent> GenerateReceiveBlockEvents(SimulationContext<EthereumNode, EthereumBlock, EthereumTransaction> context)
    {
        return context.Nodes
            .Where(node => node.Id != Block.MinerId)
            .Select(receiver => new ReceiveBlockEvent
            {
                Node = receiver,
                Block = Block,
                EventTime = EventTime + context.Network.BlockPropogationDelay(),
            })
            .ToList();
    }

    private MineBlockEvent? GenerateMineBlockEvent(SimulationContext<EthereumNode, EthereumBlock, EthereumTransaction> context, EthereumNode miner)
    {
        if (miner.HashPower <= 0)
            return null;

        var eventTime = EventTime + context.Consensus.Protocol(context, miner);

        if (eventTime > Configuration.Instance.SimulationLengthInSeconds)
            return null;

        return new MineBlockEvent(miner, eventTime);
    }
}