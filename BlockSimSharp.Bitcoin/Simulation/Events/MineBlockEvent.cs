using BlockSimSharp.Core.Configuration.Enum;
using BlockSimSharp.Core.Configuration.Model;

namespace BlockSimSharp.Bitcoin.Simulation.Events;

public class MineBlockEvent: Event
{

    public override void Handle(Context context)
    {
        var miner = context.Nodes.FirstOrDefault(node => node.NodeId == Node.NodeId);

        if (miner is null)
            throw new Exception($"Node with Id {Node.NodeId} does not exist!");

        // This check allows us to discard an event that has already been scheduled,
        // because this node has accepted a new block into the blockchain in the
        // meantime.
        var blockSequenceValid = Block.PreviousBlockId == miner.LastBlock.BlockId;
        if (!blockSequenceValid)
            return;

        // context.Statistics.TotalBlocks += 1;

        var transactionSettings = context.Settings.Get<TransactionSettings>();

        if (transactionSettings.Enabled && context.TransactionContext is not null)
        {
            var (transactions, sizeInMb) = context.TransactionContext.CollectTransactions(context, miner, EventTime);
            Block.Transactions = transactions;
            Block.UsedGas = sizeInMb;
        }

        miner.BlockChain.Add(Block);

        if (transactionSettings is { Enabled: true, Type: TransactionContextType.Light } && context.TransactionContext is not null)
        {
            context.TransactionContext.CreateTransactions(context);
        }
        
        context.Scheduler.TryScheduleReceiveBlockEvents(context, this);
        context.Scheduler.TryScheduleMineBlockEvent(context, this, miner);
    }
}