using BlockSimSharp.BitcoinBurn.Scenarios.WithNetworkDelay;
using BlockSimSharp.BitcoinBurn.Simulation;

namespace BlockSimSharp.BitcoinBurn.Scenarios.WithBurning;

// This scenario is used to determine the relation between power usage and network delay.
// Assumption is that given exactly the same "hardware", when there is no network delay
// in getting confirmation about mined blocks, the amount of power used should be exactly
// the same between each of the nodes.
public sealed class WithBurningScenario: Scenario
{
    public override string Name => nameof(WithBurningScenario);

    protected override string SettingsPath => "Scenarios/WithBurning/appsettings.json";

    protected override Nodes BuildScenarioNodes()
    {
        return new Nodes
        {
            new(0, 100, 0),
            new(1, 100, 0),
            new(2, 50, 0),
            new(3, 50, 1),
            new(4, 25, 0),
            new(5, 25, 1),
            new(6, 10, 2),
            new(7, 10, 1)
        };
    }
}