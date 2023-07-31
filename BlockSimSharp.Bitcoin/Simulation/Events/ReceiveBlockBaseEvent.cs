using BlockSimSharp.Bitcoin.Model;
using BlockSimSharp.Core;
using BlockSimSharp.Core.Configuration;
using BlockSimSharp.Core.Configuration.Enum;
using BlockSimSharp.Core.Configuration.Model;
using BlockSimSharp.Core.Simulation;

namespace BlockSimSharp.Bitcoin.Simulation.Events;

public class ReceiveBlockBaseEvent: BaseEvent<Transaction, Block, Node>
{
    public override void Handle(SimulationContext context)
    {
        var nodes = context.Get<Nodes>();

        var miner = nodes.FirstOrDefault(node => node.NodeId == Block.MinerId);
        
        if (miner is null)
            throw new Exception($"Node with Id {Node.NodeId} does not exist!");
        
        var recipient = nodes.FirstOrDefault(node => node.NodeId == Node.NodeId);
        
        if (recipient is null)
            throw new Exception($"Node with Id {Block.MinerId} does not exist!");
        
        var nextBlockInRecipientBlockChain = Block.PreviousBlockId == recipient.LastBlock.BlockId;
        
        var settings = context.Get<Settings>();
        var scheduler = context.Get<Scheduler>();
        var transactionSettings = settings.Get<TransactionSettings>();
        var transactionContext = context.TryGet<BaseTransactionContext<Transaction, Block, Node>>();
       
        // The last block in our block chain matches the last block that the new mined block
        // is based on. As such, we do not need to modify our local blockchain and we can simply
        // accept the block into our blockchain.
        if (nextBlockInRecipientBlockChain)
        {
            recipient.BlockChain.Add(Block);
            
            if (transactionSettings is { Enabled: true, Type: TransactionContextType.Full } && transactionContext is not null)
            {
                recipient.UpdateTransactionPool(Block);
            }
            
            // Once we have accepted the block, the previous scheduled event for mining a block
            // becomes invalid as such we schedule a new one immediately.
            scheduler.TryScheduleMineBlockEvent(context, this, recipient);
        }
        else
        {  
            // Otherwise we have to check whether the miner of the received block has a blockchain that is longer
            // than ours. If they have a longer blockchain, we discard our blockchain and accept their blockchain.
            if (recipient.BlockChainLength < Block.Depth)
            {
                recipient.UpdateLocalBlockChain(miner, Block.Depth + 1);
                
                if (transactionSettings is { Enabled: true, Type: TransactionContextType.Full } && transactionContext is not null)
                {
                    recipient.UpdateTransactionPool(recipient.LastBlock);
                }
                
                // As before, given that we have updated the last block of our blockchain, we have to
                // reschedule an block mining event, as the one that was scheduled previously has become 
                // invalid and will exit early during handling.
                
                scheduler.TryScheduleMineBlockEvent(context, this, recipient);
            }
            
            if (transactionSettings is { Enabled: true, Type: TransactionContextType.Full } && transactionContext is not null)
            {
                recipient.UpdateTransactionPool(Block);
            }
        }
    }
    
}