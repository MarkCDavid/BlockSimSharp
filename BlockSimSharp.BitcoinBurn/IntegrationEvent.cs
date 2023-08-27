namespace BlockSimSharp.BitcoinBurn;

public class IntegrationEvent: BaseIntegrationEvent<Action>
{
    public void Invoke()
    {
        foreach (var integrationEvent in IntegrationEvents)
        {
            integrationEvent.Invoke();
        }
    }
}
    
public class IntegrationEvent<TArguments>: BaseIntegrationEvent<Action<TArguments>>
{
    public void Invoke(TArguments arguments)
    {
        foreach (var integrationEvent in IntegrationEvents)
        {
            integrationEvent.Invoke(arguments);
        }
    }
}
    
public abstract class BaseIntegrationEvent<T> where T: Delegate
{
    public void Subscribe(T handler, int priority)
    {
        if (_integrationEvents.TryGetValue(priority, out var handlers))
        {
            handlers.Add(handler);
        }
        else
        {
            _integrationEvents[priority] = new List<T> { handler };
        }
    }

    protected IEnumerable<T> IntegrationEvents => _integrationEvents
        .SelectMany(priorityDelegates => priorityDelegates.Value);

    private readonly SortedDictionary<int, List<T>> _integrationEvents = new();
}