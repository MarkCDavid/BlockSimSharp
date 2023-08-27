using BlockSimSharp.BitcoinBurn.Simulation.Data;

namespace BlockSimSharp.BitcoinBurn.SimulationStatistics.Data;

public class Average
{
    public double BlockMiningTimeInSeconds { get; set; }

    public void Update(GlobalBlockChain globalBlockChain)
    {
        BlockMiningTimeInSeconds = CalculateAverageBlockMiningTime(globalBlockChain);
    }

    private double CalculateAverageBlockMiningTime(GlobalBlockChain globalBlockChain)
    {
        var blockMiningTimeInSeconds = 0.0;
        
        for (var blockIndex = globalBlockChain.BlockChainLength; blockIndex > 0; blockIndex--)
        {
            var currentBlock = globalBlockChain[blockIndex];
            var previousBlock = globalBlockChain[blockIndex - 1];
            blockMiningTimeInSeconds += currentBlock.MinedAt - previousBlock.MinedAt;
        }
        
        return blockMiningTimeInSeconds / globalBlockChain.BlockChainLength;
    }
}