using BlockSimSharp.Core.Configuration;
using BlockSimSharp.Core.Configuration.Model;
using Microsoft.Extensions.Configuration;

namespace BlockSimSharp.BitcoinBurn.Configuration;

public class ScenarioSettingsFactory: BaseSettingsFactory
{
    protected override void RegisterSections(IConfiguration configuration, Settings settings)
    {
        settings.Register(configuration.BindSection<ScenarioSettings>("ScenarioSettings"));
    }
}