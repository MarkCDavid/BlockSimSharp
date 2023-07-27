using BlockSimSharp.Bitcoin.Model;
using BlockSimSharp.Bitcoin.Simulation.Events;
using BlockSimSharp.Core.Configuration;
using BlockSimSharp.Core.Simulation;
using BlockSimSharp.Core.Simulation.TransactionContext;

namespace BlockSimSharp.Bitcoin.Simulation;

public class Context: BaseContext<Transaction, Block, Node, Network, Consensus, Event, Scheduler, Context>
{
    public Context(
        Settings settings, 
        BaseTransactionContext<Transaction, Block, Node, Network, Consensus, Event, Scheduler, Context>? transactionContext, 
        Network network, 
        IReadOnlyList<Node> nodes, 
        Consensus consensus, 
        Incentives incentives, 
        Scheduler scheduler) 
        : base(settings, transactionContext, network, nodes, consensus, incentives, scheduler)
    {
    }
}