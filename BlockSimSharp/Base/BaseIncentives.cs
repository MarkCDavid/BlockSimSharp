namespace BlockSimSharp.Base;

public abstract class BaseIncentives<TNode, TBlock, TTransaction>
    where TNode: BaseNode<TBlock, TTransaction>
    where TBlock: BaseBlock<TTransaction>, new()
    where TTransaction: BaseTransaction<TTransaction>
{

    public virtual void DistributeRewards(SimulationContext<TNode, TBlock, TTransaction> context)
    {
        foreach (var block in context.Consensus.GlobalBlockChain)
        {
            foreach (var node in context.Nodes)
            {
                if (block.MinerId != node.Id)
                {
                    continue;
                }

                node.Blocks += 1;
                node.Balance += Configuration.Instance.BlockReward;
                node.Balance += TransactionFee(block);
            }
        }
    }

    protected float TransactionFee(TBlock block)
    {
        return block.Transactions.Sum(transaction => transaction.Fee);
    }
}