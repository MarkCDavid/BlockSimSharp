namespace BlockSimSharp.BitcoinBurn.Simulation.Events;

public sealed class BlockMinedSimulationEvent: SimulationEvent
{
    public override void Handle(Simulator simulator)
    {
        // This check allows us to discard an event that has already been scheduled,
        // because the target node has accepted a new block into the blockchain in
        // the meantime.
        var blockSequenceValid = Block.PreviousBlock?.BlockId == Node.LastBlock.BlockId;
        if (!blockSequenceValid)
            return;
        
        Node.BlockChain.Add(Block);
        simulator.BlockMinedIntegrationEvent.Invoke(this);
    }
}