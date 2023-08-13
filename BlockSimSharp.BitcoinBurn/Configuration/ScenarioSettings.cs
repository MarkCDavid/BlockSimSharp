using BlockSimSharp.Core.Configuration.Contracts;

namespace BlockSimSharp.BitcoinBurn.Configuration;

public class ScenarioSettings: ISettings
{
    public bool RunNoNetworkDelayScenario { get; init; }
    public bool RunWithNetworkDelayScenario { get; init; }
    public bool RunWithBurningScenario { get; init; }
}