using BlockSimSharp.BitcoinBurn.Simulation;
using BlockSimSharp.BitcoinBurn.Simulation.Events;
using BlockSimSharp.BitcoinBurn.SimulationConfiguration.Factory;
using BlockSimSharp.BitcoinBurn.SimulationStatistics;
using Newtonsoft.Json;

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
        var powerUsage = new PowerUsage();
        var configuration = ConfigurationFactory.Build(configurationPath);
        var eventPool = new SimulationEventPool(configuration);
        var randomness = new Randomness(configuration);
        var network = new Network(configuration, randomness);
        var nodes = new Nodes(configuration, randomness);
        var difficulty = new Difficulty(configuration, randomness, nodes);
        var consensus = new Consensus(configuration, randomness, nodes, difficulty);
        var incentives = new Incentives(configuration, consensus);
        var statistics = new Statistics(configuration, nodes, difficulty, consensus);
        var scheduler = new Scheduler(eventPool, randomness, network, nodes, consensus);

        var simulator = new Simulator(configuration, eventPool, nodes, difficulty, consensus, incentives, scheduler, statistics);
        
        // simulator.BlockMinedIntegrationEvent.Subscribe(difficulty.OnBlockMined, 20);
        // simulator.BlockMinedIntegrationEvent.Subscribe(powerUsage.OnEvent, 30);
        simulator.BlockMinedIntegrationEvent.Subscribe(statistics.OnBlockMined, 40);
        simulator.BlockMinedIntegrationEvent.Subscribe(scheduler.OnBlockMined, 50);

        // simulator.BlockReceivedIntegrationEvent.Subscribe(powerUsage.OnEvent, 30);
        simulator.BlockMinedIntegrationEvent.Subscribe(scheduler.OnBlockReceived, 50);
        
        simulator.Simulate();

        SaveSimulationStatistics(configuration.ScenarioName, statistics);
    }

    private static void EnsureConfigurationWasProvided(IReadOnlyCollection<string> args)
    {
        if (args.Count < 1)
        {
            throw new Exception("No configurations provided!");
        }
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