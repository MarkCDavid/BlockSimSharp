using Microsoft.Extensions.Configuration;

namespace BlockSimSharp.BitcoinBurn.SimulationConfiguration.Factory;

public static class ConfigurationFactory
{
    public static Configuration Build(string configurationPath)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile(configurationPath, optional: false)
            .Build();

        var settings = new Configuration();
        
        configuration.Bind(settings);
        configuration.GetSection("Block").Bind(settings.Block);
        configuration.GetSection("Transaction").Bind(settings.Transaction);
        configuration.GetSection("RandomNumberGenerator").Bind(settings.RandomNumberGenerator);
        configuration.GetSection("Difficulty").Bind(settings.Difficulty);
        configuration.GetSection("Simulation").Bind(settings.Simulation);
        configuration.GetSection("Node").Bind(settings.Node);

        return settings;
    }
}
