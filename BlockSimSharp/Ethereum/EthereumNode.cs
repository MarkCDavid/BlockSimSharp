using BlockSimSharp.Base;

namespace BlockSimSharp.Ethereum;

public class EthereumNode: BaseNode<EthereumBlock, EthereumTransaction>
{
    public float HashPower { get; }
    public int Uncles { get; set; }
    public List<EthereumBlock> UncleChain { get; }

    public EthereumNode(int id, float hashPower) : base(id)
    {
        HashPower = hashPower;
        Uncles = 0;
        UncleChain = new List<EthereumBlock>();
    }

    public List<EthereumBlock> CollectUncles()
    {
        var maxUncles = Configuration.Instance.MaximumUncleBlocks;
        var uncles = new List<EthereumBlock>();

        for (var i = 0; i < UncleChain.Count; i++)
        {
            var uncleDepth = UncleChain[i].Depth;
            var blockDepth = LastBlock.Depth;

            if (maxUncles > 0 && uncleDepth > blockDepth - Configuration.Instance.UncleGenerations)
            {
                uncles.Add(UncleChain[i]);
                uncles.RemoveAt(i);
                i -= 1;
                maxUncles -= 1;
            }
        }

        return uncles;
    }
    
    

    public void UpdateUncleChain()
    {
        var uniqueUncleIds = new HashSet<int>();

        for (var uncleIndex = 0; uncleIndex < UncleChain.Count; uncleIndex++)
        {
            if (uniqueUncleIds.Contains(UncleChain[uncleIndex].BlockId))
            {
                UncleChain.RemoveAt(uncleIndex);
                uncleIndex--;
            }
            else
            {
                uniqueUncleIds.Add(UncleChain[uncleIndex].BlockId);
            }
        }

        for (var uncleIndex = 0; uncleIndex < UncleChain.Count; uncleIndex++)
        {
            foreach (var blockchainBlock in BlockChain)
            {
                if (UncleChain[uncleIndex].BlockId == blockchainBlock.BlockId)
                {
                    UncleChain.RemoveAt(uncleIndex);
                    uncleIndex--;
                    break;
                }
            }
        }

        for (var minerUncleIndex = 0; minerUncleIndex < UncleChain.Count; minerUncleIndex++)
        {
            var uncleFoundInBlockchain = true;
            foreach (var blockchainBlock in BlockChain)
            {
                for (var blockchainUncleIndex = 0; blockchainUncleIndex < blockchainBlock.Uncles.Count; blockchainUncleIndex++)
                {
                    if (UncleChain[minerUncleIndex].BlockId == blockchainBlock.Uncles[blockchainUncleIndex].BlockId)
                    {
                        UncleChain.RemoveAt(minerUncleIndex);
                        minerUncleIndex--;
                        uncleFoundInBlockchain = false;
                        break;
                    }
                }

                if (!uncleFoundInBlockchain)
                    break;
            }
        }
    }
}