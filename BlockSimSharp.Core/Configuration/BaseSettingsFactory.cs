using BlockSimSharp.Core.Configuration.Contracts;
using BlockSimSharp.Core.Configuration.Model;
using Microsoft.Extensions.Configuration;

namespace BlockSimSharp.Core.Configuration;

public abstract class BaseSettingsFactory
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

public static class SettingsFactoryExtensions
{
    public static TSettings BindSection<TSettings>(this IConfiguration configuration, string section)
        where TSettings: ISettings, new()
    {
        var sectionSettings = new TSettings();
        configuration.GetSection(section).Bind(sectionSettings);
        return sectionSettings;
    }
}