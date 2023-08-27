namespace BlockSimSharp.BitcoinBurn.Utility;

using System;
using System.Collections.Generic;

public class DefaultDictionary<TKey, TValue> : Dictionary<TKey, TValue> where TKey : notnull
{
    private readonly Func<TValue> _defaultValueProvider;

    public DefaultDictionary(Func<TValue> defaultValueProvider)
    {
        _defaultValueProvider = defaultValueProvider;
    }
    
    public DefaultDictionary(TValue defaultValue)
    {
        _defaultValueProvider = () => defaultValue;
    }

    public new TValue this[TKey key]
    {
        get
        {
            if (TryGetValue(key, out var value))
            {
                return value;
            }
            
            value = _defaultValueProvider();
            Add(key, value);

            return value;
        }
        set => base[key] = value;
    }

}