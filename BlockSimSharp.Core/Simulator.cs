using BlockSimSharp.Core.Configuration;
using BlockSimSharp.Core.Configuration.Model;
using BlockSimSharp.Core.Model;
using BlockSimSharp.Core.Simulation;

namespace BlockSimSharp.Core;

public class Simulator<TTransaction, TBlock, TNode>
    where TTransaction: BaseTransaction<TTransaction, TBlock, TNode>, new()
    where TBlock: BaseBlock<TTransaction, TBlock, TNode>, new()
    where TNode: BaseNode<TTransaction, TBlock, TNode>
{
    public void Simulate(SimulationContext context)
    {
        PrepareTransactionContext(context);
        ScheduleInitialEvents(context);
        RunSimulation(context);
        ResolveForks(context);
        DistributeRewards(context);
        CalculateStatistics(context);
    }

    private static void PrepareTransactionContext(SimulationContext context)
    {
        var transactionSettings = context.Get<Settings>().Get<TransactionSettings>();
        var transactionContext = context.TryGet<BaseTransactionContext<TTransaction, TBlock, TNode>>();
        
        if (transactionSettings.Enabled && transactionContext is not null)
        {
            transactionContext.CreateTransactions(context);
        }
    }

    private static void ScheduleInitialEvents(SimulationContext context)
    {
        var nodes = context.Get<BaseNodes<TTransaction, TBlock, TNode>>();
        var scheduler = context.Get<BaseScheduler<TTransaction, TBlock, TNode>>();

        foreach (var node in nodes)
        {
            scheduler.ScheduleInitialEvents(node);
        }
    }

    private static void RunSimulation(SimulationContext context)
    {
        var simulationLengthInSeconds = context.Get<Settings>().Get<SimulationSettings>().LengthInSeconds;
        var scheduler = context.Get<BaseScheduler<TTransaction, TBlock, TNode>>();
        
        while (scheduler.HasEvents())
        {
            var @event = scheduler.NextEvent();
            
            if (@event.EventTime >= simulationLengthInSeconds)
                return;
           
            @event.Handle(context);
        }
    }

    private static void ResolveForks(SimulationContext context)
    {
        var consensus = context.Get<BaseConsensus<TTransaction, TBlock, TNode>>();
        
        consensus.ResolveForks(context);
    }

    private static void DistributeRewards(SimulationContext context)
    {
        var incentives = context.Get<BaseIncentives>();
        
        incentives.DistributeRewards(context);
    }

    private static void CalculateStatistics(SimulationContext context)
    {
        var statistics = context.Get<BaseStatistics>();
        
        statistics.Calculate(context);
    }
}