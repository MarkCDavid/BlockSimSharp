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

    protected abstract void RegisterSections(IConfiguration configuration, Settings settings);

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