using BlockSimSharp.BitcoinBurn.Model;

namespace BlockSimSharp.BitcoinBurn.Simulation.Events;

public sealed class BlockMinedSimulationEvent: SimulationEvent
{
    public Block MinedBlock => Block;
    public Node Miner => Node;
    
    public override void Handle(Simulator simulator)
    {
        // This check allows us to discard an event that has already been scheduled,
        // because the target node has accepted a new block into the blockchain in
        // the meantime.
        var blockSequenceValid = Miner.LastBlock.Equals(MinedBlock.PreviousBlock);
        if (!blockSequenceValid)
        {
            return;
        }

        Miner.BlockChain.Add(MinedBlock);
        simulator.BlockMinedIntegrationEvent.Invoke(this);
    }
}