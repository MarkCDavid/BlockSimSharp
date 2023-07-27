namespace BlockSimSharp.Core.Simulation;

public class Network
{

    public float BlockPropogationDelay()
    {
        return 0;
        // return Utility.Expovariate(1.0f / Configuration.Instance.AverageBlockPropogationDelay);
    }

    public float TransactionPropogationDelay()
    {
        return 0;
        // return Utility.Expovariate(1.0f / Configuration.Instance.AverageTransactionPropogationDelay);
    }

}