using BlockSimSharp.Core.Configuration;
using Microsoft.Extensions.Configuration;

namespace BlockSimSharp.BitcoinBurn.Configuration;

public class SettingsFactory: BaseSettingsFactory
{
    protected override void RegisterSections(IConfiguration configuration, Settings settings)
    {
        base.RegisterSections(configuration, settings);
        settings.Register<BurningSettings>(configuration.BindSection<BurningSettings>("BurningSettings"));
    }
}