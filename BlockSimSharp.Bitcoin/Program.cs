using BlockSimSharp.Bitcoin.Configuration;
using BlockSimSharp.Bitcoin.Model;
using BlockSimSharp.Bitcoin.Simulation;
using BlockSimSharp.Core;
using BlockSimSharp.Core.Configuration;
using BlockSimSharp.Core.Configuration.Model;
using BlockSimSharp.Core.Simulation;
using BlockSimSharp.Core.Simulation.TransactionContext;

var settings = new SettingsFactory().Build("appsettings.json");

var simulationContext = new SimulationContext();

simulationContext.Register<Settings, Settings>(settings);
simulationContext.Register<Random, Random>(settings.Get<RandomNumberGenerationSettings>().UseStaticSeed
    ? new Random(settings.Get<RandomNumberGenerationSettings>().StaticSeed)
    : new Random());

simulationContext.Register<BaseNodes<Transaction, Block, Node>, Nodes>(new Nodes
{
    new(0, 50),
    new(1, 20),
    new(2, 30)
});

simulationContext.Register<BaseConsensus<Transaction, Block, Node>, Consensus>(new Consensus());
simulationContext.Register<BaseScheduler<Transaction, Block, Node>, Scheduler>(new Scheduler());
simulationContext
    .Register<BaseTransactionContext<Transaction, Block, Node>, BaseTransactionContext<Transaction, Block, Node>>(
        new TransactionContextFactory<Transaction, Block, Node>().BuildTransactionContext(settings));

simulationContext.Register<BaseNetwork, Network>(new Network());
simulationContext.Register<BaseIncentives, Incentives>(new Incentives());
simulationContext.Register<BaseStatistics, Statistics>(new Statistics());

var simulator = new Simulator<Transaction, Block, Node>();
simulator.Simulate(simulationContext);