
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

    private readonly char[] _delimiters = { '.' };
    public Dictionary<string, ISettings> GetAll()
    {
        return _configurationOptions.ToDictionary(
            pair => pair.Key.Split(_delimiters).Last(),
            pair => pair.Value);
    } 
}