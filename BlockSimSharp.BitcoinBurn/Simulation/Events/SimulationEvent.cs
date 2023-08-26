using BlockSimSharp.BitcoinBurn.Model;

namespace BlockSimSharp.BitcoinBurn.Simulation.Events;

public abstract class SimulationEvent
{
    public Node Node { get; init; } = null!;
    public Block Block { get; init; } = null!;
    public double EventTime { get; init; }
    
    public abstract void Handle(Simulator simulator);

}