using BlockSimSharp.BitcoinBurn.Model;
using BlockSimSharp.BitcoinBurn.Simulation.Events;
using BlockSimSharp.BitcoinBurn.SimulationConfiguration;

namespace BlockSimSharp.BitcoinBurn.Simulation;

public sealed class Difficulty
{
    private readonly Configuration _configuration;
    private readonly Randomness _randomness;
    private readonly Nodes _nodes;

    public double CurrentDifficulty { get; private set; }
    public List<DifficultyHistory> History { get; init; }

    
    public Difficulty(Configuration configuration, Randomness randomness, Nodes nodes)
    {
        _configuration = configuration;
        _randomness = randomness;
        _randomness = randomness;
        _nodes = nodes;
        
        CurrentDifficulty = nodes.TotalHashPower;
        History = new List<DifficultyHistory>
        {
            new(0, CurrentDifficulty)
        };
    }

    public double GetRelativeHashPower(Node miner)
    {
        return PercievedHashPower(miner) / CurrentDifficulty;
    }

    public double PercievedHashPower(Node miner)
    {
        return miner.HashPower;
        if (!_configuration.Difficulty.DecreaseByBurnEnabled)
        {
            return miner.HashPower;
        }

        if (!miner.ParticipatesInDifficultyDecrease)
        {
            return miner.HashPower;
        }

        if (!miner.DifficultyDecreaseDuringCurrentEpoch)
        {
            return miner.HashPower;
        }

        return miner.HashPower / miner.DifficultyDecrease;
    }
    
    public void OnBlockMined(SimulationEvent simulationEvent)
    {
        var miner = simulationEvent.Node;
        if (miner.BlockChainLength % _configuration.Difficulty.AdjustmentFrequencyInBlocks != 0)
            return;

        var epoch = miner.BlockChainLength / _configuration.Difficulty.AdjustmentFrequencyInBlocks + 1;

        CurrentDifficulty = CalculateAdjustedDifficulty(miner);
        History.Add(new DifficultyHistory(epoch, CurrentDifficulty));

        UpdateDifficultyDecreaseParticipation();
    }

    public void UpdateDifficultyDecreaseParticipation()
    {
        foreach (var node in _nodes.Where(node => node.ParticipatesInDifficultyDecrease))
        {
            var difficultyDecreaseDuringCurrentEpoch =
                _randomness.Binary(_configuration.Difficulty.ProbabilityForParticipationThisEpoch);

            var difficultyDecrease = 
                    _randomness.NextDouble(
                        _configuration.Difficulty.DecreaseAmountLowBound,
                        _configuration.Difficulty.DecreaseAmountHighBound);
            
            node.UpdateDifficultyDecreaseParticipation(difficultyDecreaseDuringCurrentEpoch, difficultyDecrease);
        }
    }

    private double CalculateAdjustedDifficulty(Node miner)
    {
        return CurrentDifficulty;
        var lastBlockOfCurrentEpoch = 
            miner.LastBlock;
        
        var lastBlockOfPreviousEpoch = 
            miner.BlockChain[miner.BlockChainLength - _configuration.Difficulty.AdjustmentFrequencyInBlocks];

        var actualMiningTime = lastBlockOfCurrentEpoch.MinedAt - lastBlockOfPreviousEpoch.MinedAt;

        var expectedMiningTime = 
            _configuration.Difficulty.AdjustmentFrequencyInBlocks * _configuration.Block.AverageIntervalInSeconds;

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