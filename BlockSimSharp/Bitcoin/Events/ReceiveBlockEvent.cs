using BlockSimSharp.Base;

namespace BlockSimSharp.Bitcoin.Events;

public class ReceiveBlockEvent: BaseEvent<BitcoinNode, BitcoinBlock, BitcoinTransaction, BitcoinScheduler>
{
    public override void Handle(SimulationContext<BitcoinNode, BitcoinBlock, BitcoinTransaction, BitcoinScheduler> context)
    {
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
            
            if (Configuration.Instance.TransactionsEnabled && Configuration.Instance.TransactionContextType == "full")
            {
                recipient.UpdateTransactionPool(Block);
            }
            
            // Once we have accepted the block, the previous scheduled event for mining a block
            // becomes invalid as such we schedule a new one immediately.
            context.Scheduler.TryScheduleMineBlockEvent(context, this, recipient);
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
                
                context.Scheduler.TryScheduleMineBlockEvent(context, this, recipient);
            }
            
            if (Configuration.Instance.TransactionsEnabled && Configuration.Instance.TransactionContextType == "full")
            {
                recipient.UpdateTransactionPool(Block);
            }
        }
    }
    
}