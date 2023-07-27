using BlockSimSharp.Bitcoin.Configuration;
using BlockSimSharp.Bitcoin.Model;
using BlockSimSharp.Bitcoin.Simulation.Events;
using BlockSimSharp.Bitcoin.Simulation.TransactionContext;
using BlockSimSharp.Core.Configuration.Model;
using BlockSimSharp.Core.Simulation;

namespace BlockSimSharp.Bitcoin.Simulation;

public class ContextFactory : BaseContextFactory<Transaction, Block, Node, Network, Consensus, Event, Scheduler, Context>
{
    public override Context BuildContext()
    {
        var settingsFactory = new SettingsFactory();
        var settings = settingsFactory.Build("appsettings.json");

        var transactionContextFactory = new TransactionContextFactory();
        var transactionContext = transactionContextFactory.BuildTransactionContext(settings.Get<TransactionSettings>());

        var network = new Network();
        
        var nodes = new List<Node>
        {
            new(0, 50),
            new(1, 20),
            new(2, 30)
        };

        var consensus = new Consensus();
        var incentives = new Incentives();
        var scheduler = new Scheduler();

        return new Context(settings, transactionContext, network, nodes, consensus, incentives, scheduler);
    }
}