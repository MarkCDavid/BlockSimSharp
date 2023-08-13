using BlockSimSharp.Core.Configuration;
using BlockSimSharp.Core.Configuration.Model;
using Microsoft.Extensions.Configuration;

namespace BlockSimSharp.BitcoinBurn.Configuration;

public class SimulationSettingsFactory: BaseSimulationSettingsFactory
{
    protected override void RegisterSections(IConfiguration configuration, Settings settings)
    {
        base.RegisterSections(configuration, settings);
        settings.Register(configuration.BindSection<BurnSettings>("BurnSettings"));
    }
}