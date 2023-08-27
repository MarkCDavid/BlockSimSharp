using BlockSimSharp.BitcoinBurn.Model;
using BlockSimSharp.BitcoinBurn.Simulation.Events;
using BlockSimSharp.BitcoinBurn.SimulationConfiguration;

namespace BlockSimSharp.BitcoinBurn.Simulation;

public sealed class Difficulty
{
    public IntegrationEvent<double> DifficultyChangeIntegrationEvent { get; } = new();
    public IntegrationEvent EpochChangeIntegrationEvent { get; } = new();

    private double CurrentDifficulty
    {
        get => _difficulty;
        set
        {
            _difficulty = value;
            DifficultyChangeIntegrationEvent.Invoke(value);
            EpochChangeIntegrationEvent.Invoke();
        }
    }
    
    public Difficulty(Configuration configuration, Randomness randomness, Nodes nodes)
    {
        _configuration = configuration;
        _randomness = randomness;
        _nodes = nodes;
    }

    public void Initialize()
    {
        CurrentDifficulty = _nodes.TotalHashPower;
    }

    public double GetRelativeHashPower(Node miner)
    {
        return PercievedHashPower(miner) / CurrentDifficulty;
    }
    
    public void OnBlockMined(BlockMinedSimulationEvent simulationEvent)
    {
        if (simulationEvent.Miner.BlockChainLength % _configuration.Difficulty.AdjustmentFrequencyInBlocks != 0)
            return;

        CurrentDifficulty = CalculateAdjustedDifficulty(simulationEvent.Miner);

        UpdateDifficultyDecreaseParticipation();
    }

    public void UpdateDifficultyDecreaseParticipation()
    {
        if (!_configuration.Difficulty.DecreaseByBurnEnabled)
        {
            return ;
        }
        
        foreach (var node in _nodes)
        {
            var participatesInReduction = _randomness.Binary(_configuration.Difficulty.ProbabilityForParticipationThisEpoch);
            
            var difficultyDecrease = participatesInReduction
                ? _randomness.NextDouble(_configuration.Difficulty.ReductionLowBound, _configuration.Difficulty.ReductionHighBound)
                : 1.0;
            
            node.UpdateDifficultyDecreaseParticipation(difficultyDecrease, participatesInReduction);
        }
    }

    private double PercievedHashPower(Node miner)
    {
        if (!_configuration.Difficulty.DecreaseByBurnEnabled)
        {
            return miner.HashPower;
        }

        return miner.HashPower * miner.DifficultyReduction;
    }

    private double CalculateAdjustedDifficulty(Node miner)
    {
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
    
    private readonly Configuration _configuration;
    private readonly Randomness _randomness;
    private readonly Nodes _nodes;
    private double _difficulty;
}