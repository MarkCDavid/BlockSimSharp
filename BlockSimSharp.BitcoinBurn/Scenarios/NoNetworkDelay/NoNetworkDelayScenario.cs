using BlockSimSharp.BitcoinBurn.Simulation;

namespace BlockSimSharp.BitcoinBurn.Scenarios.NoNetworkDelay;

// This scenario is used to determine the relation between power usage and network delay.
// Assumption is that given exactly the same "hardware", when there is no network delay
// in getting confirmation about mined blocks, the amount of power used should be exactly
// the same between each of the nodes.
public sealed class NoNetworkDelayScenario: Scenario
{
    public override string Name => nameof(NoNetworkDelayScenario);

    protected override string SettingsPath => "Scenarios/NoNetworkDelay/appsettings.json";

    protected override Nodes BuildScenarioNodes()
    {
        return new Nodes
        {
            new(0, 100, 0),
            new(1, 100, 0)
        };
    }
}