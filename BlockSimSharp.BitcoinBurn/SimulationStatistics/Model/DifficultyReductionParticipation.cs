namespace BlockSimSharp.BitcoinBurn.SimulationStatistics.Model;

public class DifficultyReductionParticipation
{
    public int Epoch { get; init; }
    public double DifficultyReduction { get; init; }
    
    public DifficultyReductionParticipation(
        Simulation.Data.DifficultyReductionParticipation difficultyReductionParticipation)
    {
        Epoch = difficultyReductionParticipation.Epoch;
        DifficultyReduction = difficultyReductionParticipation.DifficultyReduction;
    }
}