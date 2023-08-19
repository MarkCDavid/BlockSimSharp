using BlockSimSharp.Core.Configuration.Enum;
using BlockSimSharp.BitcoinBurn.SimulationConfiguration;
using BlockSimSharp.BitcoinBurn.SimulationStatistics;

namespace BlockSimSharp.BitcoinBurn.Simulation.Events;

public class MineBlockEvent: Event
{
    private readonly Configuration _configuration;
    private readonly Scheduler _scheduler;
    private readonly Difficulty _difficulty;
    private readonly Statistics _statistics;

    public MineBlockEvent(Configuration configuration, Scheduler scheduler, Difficulty difficulty, Statistics statistics)
    {
        _configuration = configuration;
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