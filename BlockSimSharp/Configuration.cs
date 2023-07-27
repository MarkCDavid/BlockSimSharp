using Microsoft.Extensions.Configuration;

namespace BlockSimSharp;

public class AppSettings
{
    public int SimulationLengthInSeconds { get; set; }

    public int AverageBlockIntervalInSeconds { get; set; }
    public float AverageBlockPropogationDelay { get; set; }
    public float AverageTransactionPropogationDelay { get; set; }
    
    public bool TransactionsEnabled { get; set; }
    
    public float BlockSizeInMb { get; set; }
    public string TransactionContextType { get; set; }
    public int TransactionsPerSecond { get; set; }
    public float AverageTransactionFee { get; set; }
    public float AverageTransactionSizeInMb { get; set; }
    public float BlockReward { get; set; }
    public bool UnclesEnabled { get; set; }
    public int MaximumUncleBlocks { get; set; }
    public int UncleGenerations { get; set; }

    public float UncleInclusionReward => BlockReward / 32;
}

public static class Configuration
{
    private static AppSettings? _instance;
    public static AppSettings Instance => _instance ??= Build("appsettings.json");

    private static AppSettings Build(string configurationSource)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile(configurationSource, optional: false, reloadOnChange: true)
            .Build();

        var appSettings = new AppSettings();
        
        configuration.GetSection("AppSettings").Bind(appSettings);
        
        return appSettings;
    }
}