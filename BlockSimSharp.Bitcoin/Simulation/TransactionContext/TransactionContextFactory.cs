using BlockSimSharp.Bitcoin.Model;
using BlockSimSharp.Bitcoin.Simulation.Events;
using BlockSimSharp.Core.Simulation.TransactionContext;

namespace BlockSimSharp.Bitcoin.Simulation.TransactionContext;

public class TransactionContextFactory: BaseTransactionContextFactory<Transaction, Block, Node, Network, Consensus, Event, Scheduler, Context>
{
    
}