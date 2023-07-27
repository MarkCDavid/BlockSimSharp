using BlockSimSharp.Core.Configuration.Model;
using BlockSimSharp.Core.Model;
using BlockSimSharp.Core.Simulation;

namespace BlockSimSharp.Core;

public class Simulator<TTransaction, TBlock, TNode, TNetwork, TConsensus, TEvent, TScheduler, TContext, TContextFactory>
    where TTransaction: BaseTransaction<TTransaction>, new()
    where TBlock: BaseBlock<TTransaction, TBlock, TNode>, new()
    where TNode: BaseNode<TTransaction, TBlock, TNode>
    where TNetwork: BaseNetwork<TTransaction, TBlock, TNode, TNetwork, TConsensus, TEvent, TScheduler, TContext>
    where TConsensus: BaseConsensus<TTransaction, TBlock, TNode, TNetwork, TConsensus, TEvent, TScheduler, TContext>
    where TScheduler: BaseScheduler<TTransaction, TBlock, TNode, TNetwork, TConsensus, TEvent, TScheduler, TContext>
    where TEvent: BaseEvent<TTransaction, TBlock, TNode, TNetwork, TConsensus, TEvent, TScheduler, TContext>
    where TContext: BaseContext<TTransaction, TBlock, TNode, TNetwork, TConsensus, TEvent, TScheduler, TContext>
    where TContextFactory: BaseContextFactory<TTransaction, TBlock, TNode, TNetwork, TConsensus, TEvent, TScheduler, TContext>
{
    public void Simulate(TContextFactory contextFactory)
    {
        var context = contextFactory.BuildContext();
        
        var simulationSettings = context.Settings.Get<SimulationSettings>();
        var transactionSettings = context.Settings.Get<TransactionSettings>();

        if (transactionSettings.Enabled && context.TransactionContext is not null)
        {
            context.TransactionContext.CreateTransactions(context);
        }
        
        foreach (var node in context.Nodes)
        {
            context.Scheduler.ScheduleInitialEvents(node);
        }
        
        var clock = 0f;
        while (clock < simulationSettings.LengthInSeconds && context.Scheduler.HasEvents())
        {
            var @event = context.Scheduler.NextEvent();  
            @event.Handle(context);
            clock = @event.EventTime;
        }
        
        context.Consensus.ForkResolution(context);
        context.Incentives.DistributeRewards(context);
        // context.Statistics.Calculate(context);
    } 
}