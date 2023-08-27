using Newtonsoft.Json;
using BlockSimSharp.BitcoinBurn.Simulation;
using BlockSimSharp.BitcoinBurn.Simulation.Events;
using BlockSimSharp.BitcoinBurn.SimulationConfiguration.Factory;
using BlockSimSharp.BitcoinBurn.SimulationStatistics;

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
        var configuration = ConfigurationFactory.Build(configurationPath);
        var randomness = new Randomness(configuration);
        var nodes = new Nodes(configuration, randomness);
        var difficulty = new Difficulty(configuration, randomness, nodes);

        var eventTimeDeltas = new EventTimeDeltas(configuration, randomness, difficulty);
        var eventPool = new SimulationEventPool(configuration);
        var scheduler = new Scheduler(eventPool, eventTimeDeltas, randomness, nodes);
        var simulator = new Simulator(eventPool);
        
        var powerUsage = new PowerUsage();
        var globalBlockChain = new GlobalBlockChain();
        var statistics = new Statistics(configuration, nodes, difficulty, globalBlockChain);
        
        // OnSimulationStarting
        simulator.SimulationStartingIntegrationEvent.Subscribe(difficulty.UpdateDifficultyDecreaseParticipation, 0);
        simulator.SimulationStartingIntegrationEvent.Subscribe(scheduler.ScheduleInitialEvents, 10);
        
        // OnBlockMined
        simulator.BlockMinedIntegrationEvent.Subscribe(difficulty.OnBlockMined, 20);
        simulator.BlockMinedIntegrationEvent.Subscribe(powerUsage.AdjustPowerUsed, 30);
        simulator.BlockMinedIntegrationEvent.Subscribe(statistics.OnBlockMined, 40);
        simulator.BlockMinedIntegrationEvent.Subscribe(scheduler.OnBlockMined, int.MaxValue);

        // OnBlockReceived
        simulator.BlockReceivedIntegrationEvent.Subscribe(powerUsage.AdjustPowerUsed, 30);
        simulator.BlockReceivedIntegrationEvent.Subscribe(scheduler.OnBlockReceived, int.MaxValue);
        
        // OnSimulationStopping
        simulator.SimulationStoppingIntegrationEvent.Subscribe(() => globalBlockChain.ResolveForks(nodes), 0);
        simulator.SimulationStoppingIntegrationEvent.Subscribe(() => globalBlockChain.DistributeRewards(configuration), 10);
        simulator.SimulationStoppingIntegrationEvent.Subscribe(() => statistics.Calculate(nodes), int.MaxValue);
        
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