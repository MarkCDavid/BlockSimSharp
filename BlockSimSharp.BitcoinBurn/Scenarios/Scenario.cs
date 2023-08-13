using BlockSimSharp.BitcoinBurn.Configuration;
using BlockSimSharp.BitcoinBurn.Model;
using BlockSimSharp.BitcoinBurn.Simulation;
using BlockSimSharp.BitcoinBurn.Simulation.Statistics;
using BlockSimSharp.Core;
using BlockSimSharp.Core.Configuration;
using BlockSimSharp.Core.Configuration.Model;
using BlockSimSharp.Core.Simulation;
using BlockSimSharp.Core.Simulation.TransactionContext;

namespace BlockSimSharp.BitcoinBurn.Scenarios;

public abstract class Scenario
{
    public abstract string Name { get; }
    protected abstract string SettingsPath { get; }
    protected abstract Nodes BuildScenarioNodes();

    public SimulationStatistics Execute()
    {
        var settings = new SimulationSettingsFactory()
            .Build(SettingsPath);

        var transactionContext = new TransactionContextFactory<Transaction, Block, Node>()
            .BuildTransactionContext(settings);
        
        var simulationContext = new SimulationContext();

        simulationContext
            .Register<Settings, Settings>(settings);
        
        simulationContext
            .Register<Randomness, Randomness>(new Randomness(settings));
        
        simulationContext
            .Register<BaseNodes<Transaction, Block, Node>, Nodes>(BuildScenarioNodes());

        simulationContext
            .Register<BaseConsensus<Transaction, Block, Node>, Consensus>(new Consensus());
        
        simulationContext
            .Register<BaseScheduler<Transaction, Block, Node>, Scheduler>(new Scheduler());
        
        simulationContext
            .Register<BaseTransactionContext<Transaction, Block, Node>, BaseTransactionContext<Transaction, Block, Node>>(transactionContext);

        simulationContext
            .Register<BaseNetwork, Network>(new Network());
        
        simulationContext
            .Register<BaseIncentives, Incentives>(new Incentives());
        
        simulationContext
            .Register<BaseStatistics, SimulationStatistics>(new SimulationStatistics());
        
        simulationContext
            .Register<Constants, Constants>(new Constants(simulationContext));

        var simulator = new Simulator<Transaction, Block, Node>();
        simulator.Simulate(simulationContext);
        
        return simulationContext.Get<SimulationStatistics>();
    }
    
}