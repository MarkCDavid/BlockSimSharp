using BlockSimSharp.Core.Configuration.Contracts;

namespace BlockSimSharp.BitcoinBurn.Configuration;

public class ScenarioSettings: ISettings
{
    public string ScenarioName { get; set; } = null!;
}