using BlockSimSharp.Core.Configuration.Contracts;

namespace BlockSimSharp.BitcoinBurn.Configuration;

public class ScenarioSettings: ISettings
{
    public bool RunNoNetworkScenario { get; init; }
    public bool RunWithNetworkScenario { get; init; }
}