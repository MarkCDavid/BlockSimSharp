using BlockSimSharp.BitcoinBurn.SimulationStatistics;

namespace BlockSimSharp.BitcoinBurn.Simulation.Events;

public sealed class MineBlockEvent: Event
{
    private readonly Scheduler _scheduler;
    private readonly Difficulty _difficulty;
    private readonly Statistics _statistics;

    public MineBlockEvent(Scheduler scheduler, Difficulty difficulty, Statistics statistics)
    {
        _scheduler = scheduler;
        _difficulty = difficulty;
        _statistics = statistics;
    }
    
    public override void Handle()
    {
        // This check allows us to discard an event that has already been scheduled,
        // because this node has accepted a new block into the blockchain in the
        // meantime.
        var blockSequenceValid = Block.PreviousBlock?.BlockId == Node.LastBlock.BlockId;
        if (!blockSequenceValid)
            return;

        _statistics.TotalBlocks += 1;
        
        Node.BlockChain.Add(Block);
        _difficulty.OnBlockMined(Node);

        _scheduler.TryScheduleReceiveBlockEvents(this);
        _scheduler.TryScheduleMineBlockEvent(this, Node);
    }
}