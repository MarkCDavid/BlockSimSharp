using BlockSimSharp.Base;

namespace BlockSimSharp.Bitcoin.Events;

public class MineBlockEvent: BaseEvent<BitcoinNode, BitcoinBlock, BitcoinTransaction, BitcoinScheduler>
{

    public override void Handle(SimulationContext<BitcoinNode, BitcoinBlock, BitcoinTransaction, BitcoinScheduler> context)
    {
        var miner = context.Nodes.FirstOrDefault(node => node.Id == Node.Id);

        if (miner is null)
            throw new Exception($"Node with Id {Node.Id} does not exist!");

        // This check allows us to discard an event that has already been scheduled,
        // because this node has accepted a new block into the blockchain in the
        // meantime.
        var blockSequenceValid = Block.PreviousBlockId == miner.LastBlock.BlockId;
        if (!blockSequenceValid)
            return;

        context.Statistics.TotalBlocks += 1;

        if (Configuration.Instance.TransactionsEnabled)
        {
            var (transactions, sizeInMb) = context.TransactionContext.CollectTransactions(miner, EventTime);
            Block.Transactions = transactions;
            Block.UsedGas = sizeInMb;
        }

        miner.BlockChain.Add(Block);

        if (Configuration.Instance.TransactionsEnabled && Configuration.Instance.TransactionContextType == "light")
        {
            context.TransactionContext.CreateTransactions(context);
        }
        
        context.Scheduler.TryScheduleReceiveBlockEvents(context, this);
        context.Scheduler.TryScheduleMineBlockEvent(context, this, miner);
    }
}