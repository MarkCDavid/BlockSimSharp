namespace BlockSimSharp.BitcoinBurn
{
    public class IntegrationEvent<TArguments>
    {
        public void Invoke(TArguments arguments)
        {
            var integrationEvents 
                = _integrationEvents.SelectMany(priorityDelegates => priorityDelegates.Value);
            
            foreach (var integrationEvent in integrationEvents)
            {
                integrationEvent.DynamicInvoke(arguments);
            }
        }

        public void Subscribe(Action<TArguments> handler, int priority)
        {
            if (_integrationEvents.TryGetValue(priority, out var handlers))
            {
                handlers.Add(handler);
            }
            else
            {
                _integrationEvents[priority] = new List<Action<TArguments>> { handler };
            }
        }

        private readonly SortedDictionary<int, List<Action<TArguments>>> _integrationEvents = new();
    }
}