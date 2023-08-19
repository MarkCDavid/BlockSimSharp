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
        
        if (nextBlockInRecipientBlockChain)
        {
            // The last block in our block chain matches the last block that the new mined block
            // is based on. As such, we do not need to modify our local blockchain and we can simply
            // accept the block into our blockchain.
            AcceptBlock();
        }
        else
        {  
            // Otherwise we have to check whether the miner of the received block has a blockchain that is longer
            // than ours. If they have a longer blockchain, we discard our blockchain and accept their blockchain.
            if (Node.BlockChainLength < Block.Depth)
            {
                AcceptChain();
            }
        }
    }

    private void AcceptChain()
    {
        Node.UpdateLocalBlockChain(Block.Miner!, Block.Depth + 1);

        // Given that we have updated the last block of our blockchain, we have to
        // reschedule the block mining event, as the one that was scheduled previously
        // has become invalid and will exit early during handling.
        _scheduler.ScheduleMineBlockEvent(this, Node);
    }

    private void AcceptBlock()
    {
        Node.BlockChain.Add(Block);

        // Once we have accepted the block, the previous scheduled event for mining a block
        // becomes invalid as such we schedule a new one immediately.
        _scheduler.ScheduleMineBlockEvent(this, Node);
    }
}