namespace BlockSimSharp.Core;

public class SimulationContext
{
    private readonly Dictionary<string, object> _contextInstances;

    public SimulationContext()
    {
        _contextInstances = new Dictionary<string, object>();
    }

    public void Register<TBase, TConcrete>(TConcrete? instance)
        where TConcrete : TBase
    {
        if (instance is null)
            return;

        var baseKey = typeof(TBase).ToString();

        if (_contextInstances.ContainsKey(baseKey))
            _contextInstances.Remove(baseKey);

        _contextInstances.Add(baseKey, instance);
        
        var concreteKey = typeof(TConcrete).ToString();

        if (baseKey == concreteKey)
            return;
        
        if (_contextInstances.ContainsKey(concreteKey))
            _contextInstances.Remove(concreteKey);

        _contextInstances.Add(concreteKey, instance);
    }

    public TBase Get<TBase>()
        where TBase : class
    {
        var key = typeof(TBase).ToString();

        if (_contextInstances.TryGetValue(key, out var contextInstance) &&
            contextInstance is TBase typedContextInstance)
            return typedContextInstance;

        throw new ArgumentOutOfRangeException(key, $"No context registered with type {key}.");
    }

    public TBase? TryGet<TBase>()
        where TBase : class
    {
        var key = typeof(TBase).ToString();

        if (_contextInstances.TryGetValue(key, out var contextInstance) &&
            contextInstance is TBase typedContextInstance)
            return typedContextInstance;

        return null;
    }
}