using BlockSimSharp.BitcoinBurn.Model;
using BlockSimSharp.BitcoinBurn.Simulation.Statistics;
using BlockSimSharp.Core;
using BlockSimSharp.Core.Configuration;
using BlockSimSharp.Core.Configuration.Enum;
using BlockSimSharp.Core.Configuration.Model;
using BlockSimSharp.Core.Simulation;

namespace BlockSimSharp.BitcoinBurn.Simulation.Events;

public class MineBlockEvent: BaseEvent<Transaction, Block, Node>
{
    public override void Handle(SimulationContext context)
    {
        // This check allows us to discard an event that has already been scheduled,
        // because this node has accepted a new block into the blockchain in the
        // meantime.
        var blockSequenceValid = Block.PreviousBlock?.BlockId == Node.LastBlock.BlockId;
        if (!blockSequenceValid)
            return;

        var consensus = context.Get<Consensus>();
        var powerCost = consensus.PowerCost(context, Node, EventTime);
        Node.TotalPowerCost += powerCost;
        
        var simulationStatistics = context.Get<SimulationStatistics>();
        simulationStatistics.TotalBlocks += 1;

        var settings = context.Get<Settings>();
        var transactionSettings = settings.Get<TransactionSettings>();
        var transactionContext = context.TryGet<BaseTransactionContext<Transaction, Block, Node>>();

        if (transactionSettings.Enabled && transactionContext is not null)
        {
            var (transactions, sizeInMb) = transactionContext.CollectTransactions(context, Node, EventTime);
            Block.Transactions = transactions;
            Block.UsedGas = sizeInMb;
        }

        Node.BlockChain.Add(Block);

        if (transactionSettings is { Enabled: true, Type: TransactionContextType.Light } && transactionContext is not null)
        {
            transactionContext.CreateTransactions(context);
        }
        
        var scheduler = context.Get<Scheduler>();
        scheduler.TryScheduleReceiveBlockEvents(context, this);
        scheduler.TryScheduleMineBlockEvent(context, this, Node);
    }
}