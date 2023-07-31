using BlockSimSharp.Bitcoin.Model;
using BlockSimSharp.Core;
using BlockSimSharp.Core.Configuration;
using BlockSimSharp.Core.Configuration.Enum;
using BlockSimSharp.Core.Configuration.Model;
using BlockSimSharp.Core.Simulation;

namespace BlockSimSharp.Bitcoin.Simulation.Events;

public class MineBlockBaseEvent: BaseEvent<Transaction, Block, Node>
{
    public override void Handle(SimulationContext context)
    {
        var settings = context.Get<Settings>();
        var scheduler = context.Get<Scheduler>();
        var nodes = context.Get<Nodes>();
        var transactionContext = context.TryGet<BaseTransactionContext<Transaction, Block, Node>>();

        var miner = nodes.FirstOrDefault(node => node.NodeId == Node.NodeId);

        if (miner is null)
            throw new Exception($"Node with Id {Node.NodeId} does not exist!");

        // This check allows us to discard an event that has already been scheduled,
        // because this node has accepted a new block into the blockchain in the
        // meantime.
        var blockSequenceValid = Block.PreviousBlockId == miner.LastBlock.BlockId;
        if (!blockSequenceValid)
            return;

        // context.Statistics.TotalBlocks += 1;

        var transactionSettings = settings.Get<TransactionSettings>();

        if (transactionSettings.Enabled && transactionContext is not null)
        {
            var (transactions, sizeInMb) = transactionContext.CollectTransactions(context, miner, EventTime);
            Block.Transactions = transactions;
            Block.UsedGas = sizeInMb;
        }

        miner.BlockChain.Add(Block);

        if (transactionSettings is { Enabled: true, Type: TransactionContextType.Light } && transactionContext is not null)
        {
            transactionContext.CreateTransactions(context);
        }
        
        scheduler.TryScheduleReceiveBlockEvents(context, this);
        scheduler.TryScheduleMineBlockEvent(context, this, miner);
    }
}