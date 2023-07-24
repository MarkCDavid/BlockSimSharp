using BlockSimSharp.Base;

namespace BlockSimSharp.Bitcoin.Events;

public class ReceiveBlockEvent: BaseEvent<BitcoinNode, BitcoinBlock>
{
    public override List<BaseEvent<BitcoinNode, BitcoinBlock>> Handle(SimulationContext<BitcoinNode, BitcoinBlock> context)
    {
        var futureEvents = new List<BaseEvent<BitcoinNode, BitcoinBlock>>();
        
        var miner = context.Nodes.FirstOrDefault(node => node.Id == Block.MinerId);
        
        if (miner is null)
            throw new Exception($"Node with Id {Node.Id} does not exist!");
        
        var recipient = context.Nodes.FirstOrDefault(node => node.Id == Node.Id);
        
        if (recipient is null)
            throw new Exception($"Node with Id {Block.MinerId} does not exist!");

        var nextBlockInRecipientBlockChain = Block.PreviousBlockId == recipient.LastBlock.BlockId;

        if (nextBlockInRecipientBlockChain)
        {
            recipient.BlockChain.Add(Block);
            
            //     if self.transaction_context and Configuration.TRANSACTION_TECHNIQUE == "Full": 
            //         self.update_transactionsPool(recipient, event.block)
            
            var mineEvent = GenerateMineBlockEvent(context, recipient);
            if (mineEvent is not null)
                futureEvents.Add(mineEvent);
        }
        else
        {  
            if (recipient.BlockChainLength < Block.Depth)
            {
                recipient.UpdateLocalBlockChain(miner, Block.Depth + 1);
                var mineEvent = GenerateMineBlockEvent(context, recipient);
                if (mineEvent is not null)
                    futureEvents.Add(mineEvent);
            }
            
            //     if self.transaction_context and Configuration.TRANSACTION_TECHNIQUE == "Full": 
            //         self.update_transactionsPool(recipient, event.block)
        }

        return futureEvents;
    }
    
    private MineBlockEvent? GenerateMineBlockEvent(SimulationContext<BitcoinNode, BitcoinBlock> context, BitcoinNode miner)
    {
        if (miner.HashPower <= 0)
            return null;

        var eventTime = EventTime + context.Consensus.Protocol(miner);

        if (eventTime > Configuration.Instance.SimulationLengthInSeconds)
            return null;

        return new MineBlockEvent(miner, eventTime);
    }
}