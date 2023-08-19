using BlockSimSharp.BitcoinBurn.SimulationConfiguration;
using BlockSimSharp.Model;

namespace BlockSimSharp.BitcoinBurn.Simulation;

public sealed class Difficulty
{
    private readonly Configuration _configuration;
    public double CurrentDifficulty { get; private set; }
    public List<DifficultyHistory> History { get; init; }
    
    public Difficulty(Configuration configuration, Nodes nodes)
    {
        _configuration = configuration;
        
        CurrentDifficulty = nodes.TotalHashPower;
        History = new List<DifficultyHistory>
        {
            new(0, CurrentDifficulty)
        };
    }

    public double GetRelativeHashPower(Node miner)
    {
        return miner.HashPower / CurrentDifficulty;
    }
    
    public void OnBlockMined(Node miner)
    {;
        
        if (miner.BlockChainLength % _configuration.Difficulty.DifficultyAdjustmentFrequencyInBlocks != 0)
            return;

        var epoch = miner.BlockChainLength / _configuration.Difficulty.DifficultyAdjustmentFrequencyInBlocks + 1;

        CurrentDifficulty = CalculateAdjustedDifficulty(miner);
        History.Add(new DifficultyHistory(epoch, CurrentDifficulty));
    }

    private double CalculateAdjustedDifficulty(Node miner)
    {
        var lastBlockOfCurrentEpoch = 
            miner.LastBlock;
        
        var lastBlockOfPreviousEpoch = 
            miner.BlockChain[miner.BlockChainLength - _configuration.Difficulty.DifficultyAdjustmentFrequencyInBlocks];

        var actualMiningTime = lastBlockOfCurrentEpoch.ExecutedAt - lastBlockOfPreviousEpoch.ExecutedAt;

        var expectedMiningTime = 
            _configuration.Difficulty.DifficultyAdjustmentFrequencyInBlocks * _configuration.Block.AverageIntervalInSeconds;

        return CurrentDifficulty * GetDifficultyChangeRatio(expectedMiningTime, actualMiningTime);
    }

    private static double GetDifficultyChangeRatio(double expectedMiningTime, double actualMiningTime)
    {
        var difficultyChangeRatio = expectedMiningTime / actualMiningTime;
        return difficultyChangeRatio switch
        {
            > 4 => 4,
            < 0.25 => 0.25,
            _ => difficultyChangeRatio
        };
    }
}

public sealed record DifficultyHistory(int Epoch, double Difficulty);