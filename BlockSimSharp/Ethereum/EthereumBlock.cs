using BlockSimSharp.Base;

namespace BlockSimSharp.Ethereum;

public class EthereumBlock: BaseBlock<EthereumTransaction>
{
    public List<EthereumBlock> Uncles { get; set; }
    public float GasLimit { get; set; }
    public float UsedGas { get; set; }

    public EthereumBlock()
    {
        Uncles = new List<EthereumBlock>();
    }
}