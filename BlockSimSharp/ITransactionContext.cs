using BlockSimSharp.Base;

namespace BlockSimSharp;

public interface ITransactionContext<TNode, TBlock, TTransaction>
    where TNode: BaseNode<TBlock, TTransaction>
    where TBlock: BaseBlock<TTransaction>, new()
    where TTransaction: BaseTransaction
{
    void CreateTransactions(SimulationContext<TNode, TBlock, TTransaction> context);
    (List<TTransaction>, float) CollectTransactions(TNode node, float currentTime);

}