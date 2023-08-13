using BlockSimSharp.Core.Configuration.Contracts;
using BlockSimSharp.Core.Configuration.Model;
using Microsoft.Extensions.Configuration;

namespace BlockSimSharp.Core.Configuration;

public abstract class BaseSimulationSettingsFactory
{
    public Settings Build(string configurationPath)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile(configurationPath, optional: false)
            .Build();

        var settings = new Settings();
        
        RegisterSections(configuration, settings);

        return settings;
    }

    protected virtual void RegisterSections(IConfiguration configuration, Settings settings)
    {
        settings.Register(configuration.BindSection<BlockSettings>("BlockSettings"));
        settings.Register(configuration.BindSection<TransactionSettings>("TransactionSettings"));
        settings.Register(configuration.BindSection<SimulationSettings>("SimulationSettings"));
        settings.Register(configuration.BindSection<RandomNumberGenerationSettings>("RandomNumberGenerationSettings"));
    }

}