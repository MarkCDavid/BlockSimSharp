using BlockSimSharp.Core.Configuration.Model;
using BlockSimSharp.Core.Model;
using BlockSimSharp.Core.Simulation.TransactionContext;

namespace BlockSimSharp.Core.Simulation;

public abstract class BaseNetwork<TTransaction, TBlock, TNode, TNetwork, TConsensus, TEvent, TScheduler, TContext>
    where TTransaction: BaseTransaction<TTransaction>, new()
    where TBlock: BaseBlock<TTransaction, TBlock, TNode>, new()
    where TNode: BaseNode<TTransaction, TBlock, TNode>
    where TNetwork: BaseNetwork<TTransaction, TBlock, TNode, TNetwork, TConsensus, TEvent, TScheduler, TContext>
    where TConsensus: BaseConsensus<TTransaction, TBlock, TNode, TNetwork, TConsensus, TEvent, TScheduler, TContext>
    where TScheduler: BaseScheduler<TTransaction, TBlock, TNode, TNetwork, TConsensus, TEvent, TScheduler, TContext>
    where TEvent: BaseEvent<TTransaction, TBlock, TNode, TNetwork, TConsensus, TEvent, TScheduler, TContext>
    where TContext: BaseContext<TTransaction, TBlock, TNode, TNetwork, TConsensus, TEvent, TScheduler, TContext>
{

    public float BlockPropogationDelay(TContext context)
    {
        return Utility.Expovariate(1.0f / context.Settings.Get<BlockSettings>().AveragePropogationDelay);
    }

    public float TransactionPropogationDelay(TContext context)
    {
        return Utility.Expovariate(1.0f / context.Settings.Get<TransactionSettings>().AveragePropogationDelay);
    }

}