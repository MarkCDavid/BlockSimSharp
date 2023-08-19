namespace BlockSimSharp.BitcoinBurn.Simulation.Events;

public sealed class ReceiveBlockEvent: Event
{
    private readonly Scheduler _scheduler;

    public ReceiveBlockEvent(Scheduler scheduler)
    {
        _scheduler = scheduler;
    }

    public override void Handle()
    {
        var nextBlockInRecipientBlockChain = Block.PreviousBlock?.BlockId == Node.LastBlock.BlockId;
        
        // The last block in our block chain matches the last block that the new mined block
        // is based on. As such, we do not need to modify our local blockchain and we can simply
        // accept the block into our blockchain.
        if (nextBlockInRecipientBlockChain)
        {
            Node.BlockChain.Add(Block);
            
            // Once we have accepted the block, the previous scheduled event for mining a block
            // becomes invalid as such we schedule a new one immediately.
            _scheduler.TryScheduleMineBlockEvent(this, Node);
        }
        else
        {  
            // Otherwise we have to check whether the miner of the received block has a blockchain that is longer
            // than ours. If they have a longer blockchain, we discard our blockchain and accept their blockchain.
            if (Node.BlockChainLength < Block.Depth)
            {
                Node.UpdateLocalBlockChain(Block.Miner, Block.Depth + 1);
                
                // As before, given that we have updated the last block of our blockchain, we have to
                // reschedule an block mining event, as the one that was scheduled previously has become 
                // invalid and will exit early during handling.
                
                _scheduler.TryScheduleMineBlockEvent(this, Node);
            }
        }
    }
    
}