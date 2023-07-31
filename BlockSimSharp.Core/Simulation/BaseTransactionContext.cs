using BlockSimSharp.Core.Model;

namespace BlockSimSharp.Core.Simulation;

public abstract class BaseTransactionContext<TTransaction, TBlock, TNode>
    where TTransaction: BaseTransaction<TTransaction>, new()
    where TBlock: BaseBlock<TTransaction, TBlock, TNode>, new()
    where TNode: BaseNode<TTransaction, TBlock, TNode>
{
    public abstract void CreateTransactions(SimulationContext context);
    public abstract (List<TTransaction>, float) CollectTransactions(SimulationContext context, TNode node, float currentTime);
}