using BlockSimSharp.BitcoinBurn.Model;
using BlockSimSharp.Core;
using BlockSimSharp.Core.Configuration;
using BlockSimSharp.Core.Configuration.Enum;
using BlockSimSharp.Core.Configuration.Model;
using BlockSimSharp.Core.Simulation;

namespace BlockSimSharp.BitcoinBurn.Simulation.Events;

public class ReceiveBlockEvent: BaseEvent<Transaction, Block, Node>
{
    public override void Handle(SimulationContext context)
    {
        var nextBlockInRecipientBlockChain = Block.PreviousBlock?.BlockId == Node.LastBlock.BlockId;
        
        var settings = context.Get<Settings>();
        var scheduler = context.Get<Scheduler>();
        var transactionSettings = settings.Get<TransactionSettings>();
        var transactionContext = context.TryGet<BaseTransactionContext<Transaction, Block, Node>>();

        if (Node.CurrentlyMinedBlock is not null)
        {
            var powerConsumptionPerHashPower = 0.3;
            
            var blockMiningStartTime = Node.CurrentlyMinedBlock.ScheduledTimestamp;
            var currentSimulationTime = EventTime;

            var totalTimeSpentMiningABlockInSeconds = currentSimulationTime - blockMiningStartTime;
            var totalEnergySpentMiningABlock = totalTimeSpentMiningABlockInSeconds * powerConsumptionPerHashPower;
        }
       
        // The last block in our block chain matches the last block that the new mined block
        // is based on. As such, we do not need to modify our local blockchain and we can simply
        // accept the block into our blockchain.
        if (nextBlockInRecipientBlockChain)
        {
            Node.BlockChain.Add(Block);
            
            if (transactionSettings is { Enabled: true, Type: TransactionContextType.Full } && transactionContext is not null)
            {
                Node.UpdateTransactionPool(Block);
            }
            
            // Once we have accepted the block, the previous scheduled event for mining a block
            // becomes invalid as such we schedule a new one immediately.
            scheduler.TryScheduleMineBlockEvent(context, this, Node);
        }
        else
        {  
            // Otherwise we have to check whether the miner of the received block has a blockchain that is longer
            // than ours. If they have a longer blockchain, we discard our blockchain and accept their blockchain.
            if (Node.BlockChainLength < Block.Depth)
            {
                Node.UpdateLocalBlockChain(Block.Miner, Block.Depth + 1);
                
                if (transactionSettings is { Enabled: true, Type: TransactionContextType.Full } && transactionContext is not null)
                {
                    Node.UpdateTransactionPool(Node.LastBlock);
                }
                
                // As before, given that we have updated the last block of our blockchain, we have to
                // reschedule an block mining event, as the one that was scheduled previously has become 
                // invalid and will exit early during handling.
                
                scheduler.TryScheduleMineBlockEvent(context, this, Node);
            }
            
            if (transactionSettings is { Enabled: true, Type: TransactionContextType.Full } && transactionContext is not null)
            {
                Node.UpdateTransactionPool(Block);
            }
        }
    }
    
}