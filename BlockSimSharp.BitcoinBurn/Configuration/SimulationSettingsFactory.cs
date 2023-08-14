using BlockSimSharp.Core.Configuration;
using Microsoft.Extensions.Configuration;

namespace BlockSimSharp.BitcoinBurn.Configuration;

public class SimulationSettingsFactory: BaseSimulationSettingsFactory
{
    protected override void RegisterSections(IConfiguration configuration, Settings settings)
    {
        base.RegisterSections(configuration, settings);
        settings.Register(configuration.BindSection<ScenarioSettings>("ScenarioSettings"));
        settings.Register(configuration.BindSection<DifficultySettings>("DifficultySettings"));
        settings.Register(configuration.BindSection<BurnSettings>("BurnSettings"));
        settings.Register(configuration.BindSection<NodeSettings>("NodeSettings"));
    }
}