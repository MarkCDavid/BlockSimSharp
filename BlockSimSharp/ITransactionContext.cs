using BlockSimSharp.Base;

namespace BlockSimSharp;

public interface ITransactionContext<TNode, TBlock, TTransaction, TScheduler>
    where TNode: BaseNode<TBlock, TTransaction>
    where TBlock: BaseBlock<TTransaction>, new()
    where TTransaction: BaseTransaction<TTransaction>
    where TScheduler: BaseScheduler<TBlock, TNode, TTransaction, TScheduler>
{
    void CreateTransactions(SimulationContext<TNode, TBlock, TTransaction, TScheduler> context);
    (List<TTransaction>, float) CollectTransactions(TNode node, float currentTime);

}