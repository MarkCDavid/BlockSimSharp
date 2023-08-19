using BlockSimSharp.BitcoinBurn.Simulation.Context.Transaction;
using BlockSimSharp.BitcoinBurn.SimulationConfiguration;
using BlockSimSharp.Core.Configuration.Enum;

namespace BlockSimSharp.BitcoinBurn.Simulation.Events;

public class ReceiveBlockEvent: Event
{
    private readonly Configuration _configuration;
    private readonly Scheduler _scheduler;
    private readonly TransactionContext? _transactionContext;

    public ReceiveBlockEvent(Configuration configuration, Scheduler scheduler, TransactionContext? transactionContext)
    {
        _configuration = configuration;
        _scheduler = scheduler;
        _transactionContext = transactionContext;
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
            
            if (_configuration.Transaction.Enabled && _configuration.Transaction.Type == TransactionContextType.Full && _transactionContext is not null)
            {
                Node.UpdateTransactionPool(Block);
            }
            
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
                
                if (_configuration.Transaction.Enabled && _configuration.Transaction.Type == TransactionContextType.Full && _transactionContext is not null)
                {
                    Node.UpdateTransactionPool(Node.LastBlock);
                }
                
                // As before, given that we have updated the last block of our blockchain, we have to
                // reschedule an block mining event, as the one that was scheduled previously has become 
                // invalid and will exit early during handling.
                
                _scheduler.TryScheduleMineBlockEvent(this, Node);
            }
            
            if (_configuration.Transaction.Enabled && _configuration.Transaction.Type == TransactionContextType.Full && _transactionContext is not null)
            {
                Node.UpdateTransactionPool(Block);
            }
        }
    }
    
}