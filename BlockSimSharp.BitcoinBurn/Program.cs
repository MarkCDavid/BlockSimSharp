﻿using BlockSimSharp.BitcoinBurn.Simulation;
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
        var configuration = ConfigurationFactory.Build(configurationPath);
        var randomness = new Randomness(configuration);
        var network = new Network(configuration, randomness);
        var nodes = new Nodes(configuration, randomness);
        var difficulty = new Difficulty(configuration, nodes);
        var consensus = new Consensus(configuration, difficulty, randomness, nodes);
        var statistics = new Statistics(configuration, difficulty, consensus);
        var scheduler = new Scheduler(configuration, consensus, nodes, network, randomness, difficulty, statistics);
        var incentives = new Incentives(configuration, consensus);

        var simulator = new Simulator(configuration, nodes, scheduler, consensus, incentives, statistics);
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