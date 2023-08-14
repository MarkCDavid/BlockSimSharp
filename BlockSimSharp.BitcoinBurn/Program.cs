using BlockSimSharp.BitcoinBurn.Configuration;
using BlockSimSharp.BitcoinBurn.Model;
using BlockSimSharp.BitcoinBurn.Simulation;
using BlockSimSharp.BitcoinBurn.Simulation.Statistics;
using BlockSimSharp.Core;
using BlockSimSharp.Core.Configuration;
using BlockSimSharp.Core.Simulation;
using BlockSimSharp.Core.Simulation.TransactionContext;
using Newtonsoft.Json;
using Node = BlockSimSharp.BitcoinBurn.Model.Node;

namespace BlockSimSharp.BitcoinBurn;

internal abstract class Program
{
    public static void Main(string[] args)
    {
        EnsureConfigurationWasProvided(args);

        foreach (var configurationPath in args)
        {
            RunScenario(configurationPath);
        }
    }

    private static void RunScenario(string configurationPath)
    {
        var settings = new SimulationSettingsFactory().Build(configurationPath);

        var simulationContext = BuildSimulationContext(settings);

        new Simulator<Transaction, Block, Node>().Simulate(simulationContext);

        SaveSimulationStatistics(
            settings.Get<ScenarioSettings>().ScenarioName,
            simulationContext.Get<Statistics>());
    }

    private static void EnsureConfigurationWasProvided(string[] args)
    {
        if (args.Length < 1)
        {
            throw new Exception("No configurations provided!");
        }
    }

    private static SimulationContext BuildSimulationContext(Settings settings)
    {
        var simulationContext = new SimulationContext();
        simulationContext.Register<Settings, Settings>(settings);
        
        var randomness = new Randomness(settings);
        simulationContext.Register<Randomness, Randomness>(randomness);

        var nodes = BuildNodes(settings, randomness);
        simulationContext.Register<BaseNodes<Transaction, Block, Node>, Nodes>(nodes);

        var difficulty = new Difficulty(nodes);
        simulationContext.Register<Difficulty, Difficulty>(difficulty);
        
        var transactionContext = new TransactionContextFactory<Transaction, Block, Node>().BuildTransactionContext(settings);
        simulationContext.Register<BaseTransactionContext<Transaction, Block, Node>, BaseTransactionContext<Transaction, Block, Node>>(transactionContext);

        simulationContext
            .Register<BaseConsensus<Transaction, Block, Node>, Consensus>(new Consensus());

        simulationContext
            .Register<BaseScheduler<Transaction, Block, Node>, Scheduler>(new Scheduler());

        simulationContext
            .Register<BaseNetwork, Network>(new Network());

        simulationContext
            .Register<BaseIncentives, Incentives>(new Incentives());

        simulationContext
            .Register<BaseStatistics, Statistics>(new Statistics());

        simulationContext
            .Register<Constants, Constants>(new Constants(simulationContext));
        
        return simulationContext;
    }

    private static Nodes BuildNodes(Settings settings, Randomness randomness)
    {
        var nodeSettings = settings.Get<NodeSettings>();
        var nodes = new Nodes();

        for (var nodeId = 0; nodeId < nodeSettings.StartingNodeCount; nodeId++)
        {
            // Aoki et al. (2019) SimBlock
            var hashPower = randomness.NextGaussian() * nodeSettings.StandardDeviationOfHashRate + nodeSettings.AverageHashRate;
            hashPower = MathF.Max(hashPower, 1);
            nodes.Add(new Node(nodeId, hashPower));
        }
        
        return nodes;
    }

    private static void SaveSimulationStatistics(string scenarioName, Statistics statistics)
    {
        var homePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        var directoryPath = Path.Combine(homePath, "BlockSimSharp", scenarioName);
    
        Directory.CreateDirectory(directoryPath);
    
        var fileName = $"{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}.json";
        var filePath = Path.Combine(directoryPath, fileName);
    
        File.WriteAllText(
            filePath, 
            JsonConvert.SerializeObject(statistics, Formatting.Indented)
        );
    }
}