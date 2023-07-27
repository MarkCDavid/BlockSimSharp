using BlockSimSharp.Core.Model;
using BlockSimSharp.Core.Simulation.TransactionContext;
using BlockSimSharp.Core.Configuration;
using BlockSimSharp.Core.Configuration.Model;

namespace BlockSimSharp.Core.Simulation;

public abstract class BaseContext<TTransaction, TBlock, TNode, TNetwork, TConsensus, TEvent, TScheduler, TContext>
    where TTransaction: BaseTransaction<TTransaction>, new()
    where TBlock: BaseBlock<TTransaction, TBlock, TNode>, new()
    where TNode: BaseNode<TTransaction, TBlock, TNode>
    where TNetwork: BaseNetwork<TTransaction, TBlock, TNode, TNetwork, TConsensus, TEvent, TScheduler, TContext>
    where TConsensus: BaseConsensus<TTransaction, TBlock, TNode, TNetwork, TConsensus, TEvent, TScheduler, TContext>
    where TScheduler: BaseScheduler<TTransaction, TBlock, TNode, TNetwork, TConsensus, TEvent, TScheduler, TContext>
    where TEvent: BaseEvent<TTransaction, TBlock, TNode, TNetwork, TConsensus, TEvent, TScheduler, TContext>
    where TContext: BaseContext<TTransaction, TBlock, TNode, TNetwork, TConsensus, TEvent, TScheduler, TContext>
{
    
    public Settings Settings { get; }
    
    public Random Random { get; }
    public BaseTransactionContext<TTransaction, TBlock, TNode, TNetwork, TConsensus, TEvent, TScheduler, TContext>? TransactionContext { get; }
    
    public TNetwork Network { get; }
    public IReadOnlyList<TNode> Nodes { get; }
    public TConsensus Consensus { get; }
    public BaseIncentives<TTransaction, TBlock, TNode, TNetwork, TConsensus, TEvent, TScheduler, TContext> Incentives { get; }
    public TScheduler Scheduler { get; }
    // public Statistics.Statistics<TNode, TBlock, TTransaction, TScheduler> Statistics { get; }

    public BaseContext(
        Settings settings,
        BaseTransactionContext<TTransaction, TBlock, TNode, TNetwork, TConsensus, TEvent, TScheduler, TContext>? transactionContext,
        TNetwork network, 
        IReadOnlyList<TNode> nodes, 
        TConsensus consensus,
        BaseIncentives<TTransaction, TBlock, TNode, TNetwork, TConsensus, TEvent, TScheduler, TContext> incentives,
        TScheduler scheduler
        // Statistics.Statistics<TNode, TBlock, TTransaction, TScheduler> statistics
        )
    {
        Settings = settings;
        
        Random = BuildRandom(Settings.Get<RandomNumberGenerationSettings>());
        
        TransactionContext = transactionContext;
        Network = network;
        Nodes = nodes;
        Consensus = consensus;
        Scheduler = scheduler;
        Incentives = incentives;
        
        // Statistics = statistics;
    }

    private Random BuildRandom(RandomNumberGenerationSettings randomNumberGenerationSettings)
    {
        if (randomNumberGenerationSettings.UseStaticSeed)
        {
            return new Random(randomNumberGenerationSettings.StaticSeed);
        }

        return new Random();
    }
}