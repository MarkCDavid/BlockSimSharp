using BlockSimSharp.BitcoinBurn.Configuration;
using BlockSimSharp.BitcoinBurn.Model;
using BlockSimSharp.BitcoinBurn.Simulation;
using BlockSimSharp.BitcoinBurn.Simulation.Statistics;
using BlockSimSharp.Core;
using BlockSimSharp.Core.Configuration;
using BlockSimSharp.Core.Configuration.Model;
using BlockSimSharp.Core.Simulation;
using BlockSimSharp.Core.Simulation.TransactionContext;
using Newtonsoft.Json;

var settings = new SettingsFactory().Build("appsettings.json");
var burnSettings = settings.Get<BurnSettings>();

var simulationContext = new SimulationContext();

simulationContext.Register<Settings, Settings>(settings);
simulationContext.Register<Random, Random>(settings.Get<RandomNumberGenerationSettings>().UseStaticSeed
    ? new Random(settings.Get<RandomNumberGenerationSettings>().StaticSeed)
    : new Random());

simulationContext.Register<BaseNodes<Transaction, Block, Node>, Nodes>(new Nodes
{
    new(0, 100, 0),
    new(1, 90, 0),
    new(2, 100, 0),
    new(3, 85, 0),
    new(4, 90, 0),
    new(5, 100, 1),
    new(6, 30, 1),
    new(7, 40, 2),
    new(8, 20, 2),
    new(9, 10, 3)
});

simulationContext.Register<BaseConsensus<Transaction, Block, Node>, Consensus>(new Consensus());
simulationContext.Register<BaseScheduler<Transaction, Block, Node>, Scheduler>(new Scheduler());
simulationContext
    .Register<BaseTransactionContext<Transaction, Block, Node>, BaseTransactionContext<Transaction, Block, Node>>(
        new TransactionContextFactory<Transaction, Block, Node>().BuildTransactionContext(settings));

simulationContext.Register<BaseNetwork, Network>(new Network());
simulationContext.Register<BaseIncentives, Incentives>(new Incentives());
simulationContext.Register<BaseStatistics, SimulationStatistics>(new SimulationStatistics());
simulationContext.Register<Constants, Constants>(new Constants(simulationContext));

var simulator = new Simulator<Transaction, Block, Node>();
simulator.Simulate(simulationContext);

var simulationStatistics = simulationContext.Get<SimulationStatistics>();

File.WriteAllText(
    $"{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}.json", 
    JsonConvert.SerializeObject(simulationStatistics, Formatting.Indented)
);