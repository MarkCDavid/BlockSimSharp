using BlockSimSharp.BitcoinBurn.Configuration;
using BlockSimSharp.BitcoinBurn.Model;
using BlockSimSharp.Core;
using BlockSimSharp.Core.Configuration;
using BlockSimSharp.Core.Configuration.Model;

namespace BlockSimSharp.BitcoinBurn.Simulation;

public class Difficulty
{
    public float CurrentDifficulty { get; private set; }
    public Dictionary<int, float> DifficultyHistory { get; init; }
    
    public Difficulty(Nodes nodes)
    {
        CurrentDifficulty = nodes.Sum(node => node.HashPower);
        DifficultyHistory = new Dictionary<int, float>
        {
            {0, CurrentDifficulty}
        };
    }

    public float GetRelativeHashPower(Node miner)
    {
        return miner.HashPower / CurrentDifficulty;
    }
    
    public void OnBlockMined(Node miner, SimulationContext context)
    {
        var settings = context.Get<Settings>();
        var difficultySettings = settings.Get<DifficultySettings>();
        
        if (miner.BlockChainLength % difficultySettings.DifficultyAdjustmentFrequencyInBlocks != 0)
            return;

        var epoch = miner.BlockChainLength / difficultySettings.DifficultyAdjustmentFrequencyInBlocks + 1;

        CurrentDifficulty = CalculateAdjustedDifficulty(miner, context);
        DifficultyHistory.Add(epoch, CurrentDifficulty);
    }

    private float CalculateAdjustedDifficulty(Node miner, SimulationContext context)
    {
        var settings = context.Get<Settings>();
        var difficultySettings = settings.Get<DifficultySettings>();
        var blockSettings = settings.Get<BlockSettings>();

        var lastBlockOfCurrentEpoch = 
            miner.LastBlock;
        
        var lastBlockOfPreviousEpoch = 
            miner.BlockChain[miner.BlockChainLength - difficultySettings.DifficultyAdjustmentFrequencyInBlocks];

        var actualMiningTime = lastBlockOfCurrentEpoch.Timestamp - lastBlockOfPreviousEpoch.Timestamp;

        var expectedMiningTime = 
            difficultySettings.DifficultyAdjustmentFrequencyInBlocks * blockSettings.AverageIntervalInSeconds;

        return CurrentDifficulty * GetDifficultyChangeRatio(expectedMiningTime, actualMiningTime);
    }

    private float GetDifficultyChangeRatio(float expectedMiningTime, float actualMiningTime)
    {
        var difficultyChangeRatio = expectedMiningTime / actualMiningTime;

        if (difficultyChangeRatio > 4)
        {
            return 4;
        }

        if (difficultyChangeRatio < 0.25)
        {
            return 0.25f;
        }

        return difficultyChangeRatio;
    }

    
    
}