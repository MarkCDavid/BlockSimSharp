using BlockSimSharp.Base;

namespace BlockSimSharp.Bitcoin.Events;

public class ReceiveBlockEvent: BaseEvent<BitcoinNode, BitcoinBlock, BitcoinTransaction>
{
    public override List<BaseEvent<BitcoinNode, BitcoinBlock, BitcoinTransaction>> Handle(SimulationContext<BitcoinNode, BitcoinBlock, BitcoinTransaction> context)
    {
        var futureEvents = new List<BaseEvent<BitcoinNode, BitcoinBlock, BitcoinTransaction>>();
        
        var miner = context.Nodes.FirstOrDefault(node => node.Id == Block.MinerId);
        
        if (miner is null)
            throw new Exception($"Node with Id {Node.Id} does not exist!");
        
        var recipient = context.Nodes.FirstOrDefault(node => node.Id == Node.Id);
        
        if (recipient is null)
            throw new Exception($"Node with Id {Block.MinerId} does not exist!");
        
        var nextBlockInRecipientBlockChain = Block.PreviousBlockId == recipient.LastBlock.BlockId;
       
        // The last block in our block chain matches the last block that the new mined block
        // is based on. As such, we do not need to modify our local blockchain and we can simply
        // accept the block into our blockchain.
        if (nextBlockInRecipientBlockChain)
        {
            recipient.BlockChain.Add(Block);
            
            //     if self.transaction_context and Configuration.TRANSACTION_TECHNIQUE == "Full": 
            //         self.update_transactionsPool(recipient, event.block)
            
            
            // Once we have accepted the block, the previous scheduled event for mining a block
            // becomes invalid as such we schedule a new one immediately.
            var mineEvent = GenerateMineBlockEvent(context, recipient);
            if (mineEvent is not null)
                futureEvents.Add(mineEvent);
        }
        else
        {  
            // Otherwise we have to check whether the miner of the received block has a blockchain that is longer
            // than ours. If they have a longer blockchain, we discard our blockchain and accept their blockchain.
            if (recipient.BlockChainLength < Block.Depth)
            {
                recipient.UpdateLocalBlockChain(miner, Block.Depth + 1);
                
                // As before, given that we have updated the last block of our blockchain, we have to
                // reschedule an block mining event, as the one that was scheduled previously has become 
                // invalid and will exit early during handling.
                var mineEvent = GenerateMineBlockEvent(context, recipient);
                if (mineEvent is not null)
                    futureEvents.Add(mineEvent);
            }
            
            //     if self.transaction_context and Configuration.TRANSACTION_TECHNIQUE == "Full": 
            //         self.update_transactionsPool(recipient, event.block)
        }

        return futureEvents;
    }
    
    private MineBlockEvent? GenerateMineBlockEvent(SimulationContext<BitcoinNode, BitcoinBlock, BitcoinTransaction> context, BitcoinNode miner)
    {
        if (miner.HashPower <= 0)
            return null;

        var eventTime = EventTime + context.Consensus.Protocol(miner);

        if (eventTime > Configuration.Instance.SimulationLengthInSeconds)
            return null;

        return new MineBlockEvent(miner, eventTime);
    }
}