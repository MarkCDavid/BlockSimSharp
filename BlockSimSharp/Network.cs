namespace BlockSimSharp;

public class Network
{

    public float BlockPropogationDelay()
    {
        return Utility.Expovariate(1.0f / Configuration.Instance.AverageBlockPropogationDelay);
    }

    public float TransactionPropogationDelay()
    {
        return Utility.Expovariate(1.0f / Configuration.Instance.AverageTransactionPropogationDelay);
    }
//     import random
// import Configuration
//
// class Network:
//
//     def __init__(self: 'Network') -> 'None':
//         pass
//     
//     def block_propogation_delay(self: 'Network') -> 'float':
//         return random.expovariate(1 / Configuration.AVERAGE_BLOCK_PROPOGATION_DELAY)
//
//     def transaction_propogation_delay():
//     	return random.expovariate(1 / Configuration.AVERAGE_TRANSACTION_PROPOGATION_DELAY)

}