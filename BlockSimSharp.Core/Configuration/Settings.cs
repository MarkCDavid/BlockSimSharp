
using BlockSimSharp.Core.Configuration.Contracts;

namespace BlockSimSharp.Core.Configuration;

public class Settings
{
    private readonly Dictionary<string, ISettings> _configurationOptions;

    public Settings()
    {
        _configurationOptions = new Dictionary<string, ISettings>();
    }

    public void Register<TSettings>(TSettings instance) 
        where TSettings: class, ISettings
    {
        var key = typeof(TSettings).ToString();
        
        if (_configurationOptions.ContainsKey(key))
            _configurationOptions.Remove(key);
        
        _configurationOptions.Add(key, instance);
    }

    public TSettings Get<TSettings>()
        where TSettings: class, ISettings
    {
        var key = typeof(TSettings).ToString();
        
        if (_configurationOptions.TryGetValue(key, out var settings) && settings is TSettings typedSettings)
            return typedSettings;

        throw new ArgumentOutOfRangeException(key, $"No settings registered with type {key}.");
    }
}



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

