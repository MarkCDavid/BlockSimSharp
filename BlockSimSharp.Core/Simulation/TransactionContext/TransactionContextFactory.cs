using BlockSimSharp.Core.Configuration;
using BlockSimSharp.Core.Configuration.Enum;
using BlockSimSharp.Core.Configuration.Model;
using BlockSimSharp.Core.Model;

namespace BlockSimSharp.Core.Simulation.TransactionContext;

public class TransactionContextFactory<TTransaction, TBlock, TNode>
    where TTransaction : BaseTransaction<TTransaction, TBlock, TNode>, new()
    where TBlock : BaseBlock<TTransaction, TBlock, TNode>, new()
    where TNode : BaseNode<TTransaction, TBlock, TNode>
{
    public virtual BaseTransactionContext<TTransaction, TBlock, TNode>? BuildTransactionContext(Settings settings)
    {
        var transactionSettings = settings.Get<TransactionSettings>();
        
        if (!transactionSettings.Enabled)
            return null;

        return transactionSettings.Type switch
        {
            TransactionContextType.Light => new LightBaseTransactionContext<TTransaction, TBlock, TNode>(),
            TransactionContextType.Full => new FullBaseTransactionContext<TTransaction, TBlock, TNode>(),
            _ => throw new ArgumentOutOfRangeException(nameof(TransactionSettings.Type))
        };
    }
}