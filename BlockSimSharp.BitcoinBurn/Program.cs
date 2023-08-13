using BlockSimSharp.BitcoinBurn.Configuration;
using BlockSimSharp.BitcoinBurn.Scenarios;
using BlockSimSharp.BitcoinBurn.Scenarios.NoNetworkDelay;
using BlockSimSharp.BitcoinBurn.Scenarios.WithNetworkDelay;
using BlockSimSharp.BitcoinBurn.Simulation.Statistics;
using Newtonsoft.Json;

void RunScenario<TScenario>()
    where TScenario: Scenario, new()
{
    var scenario = new TScenario();
    var statistics = scenario.Execute();
    SaveSimulationData(scenario, statistics);
}

void SaveSimulationData<TScenario>(TScenario scenario, SimulationStatistics statistics)
    where TScenario: Scenario
{
    var homePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
    var directoryPath = Path.Combine(homePath, "BlockSimSharp", scenario.Name);
    
    Directory.CreateDirectory(directoryPath);
    
    var fileName = $"{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}.json";
    var filePath = Path.Combine(directoryPath, fileName);
    
    File.WriteAllText(
        filePath, 
        JsonConvert.SerializeObject(statistics, Formatting.Indented)
    );
}

var scenarioSettings = new ScenarioSettingsFactory()
    .Build("ScenarioSettings.json")
    .Get<ScenarioSettings>();

if (scenarioSettings.RunNoNetworkScenario)
    RunScenario<NoNetworkDelayScenario>();

if (scenarioSettings.RunWithNetworkScenario)
    RunScenario<WithNetworkDelayScenario>();
