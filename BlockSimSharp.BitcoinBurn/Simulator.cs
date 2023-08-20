using BlockSimSharp.BitcoinBurn.Simulation;
using BlockSimSharp.BitcoinBurn.Simulation.Events;
using BlockSimSharp.BitcoinBurn.SimulationConfiguration;
using BlockSimSharp.BitcoinBurn.SimulationStatistics;

namespace BlockSimSharp.BitcoinBurn;

public class Simulator
{
    private readonly Configuration _configuration;
    private readonly EventPool _eventPool;
    private readonly Nodes _nodes;
    private readonly Difficulty _difficulty;
    private readonly Consensus _consensus;
    private readonly Incentives _incentives;
    private readonly Scheduler _scheduler;
    private readonly Statistics _statistics;

    public Simulator(
        Configuration configuration, 
        EventPool eventPool,
        Nodes nodes, 
        Difficulty difficulty,
        Consensus consensus, 
        Incentives incentives,
        Scheduler scheduler, 
        Statistics statistics)
    {
        _configuration = configuration;
        _eventPool = eventPool;
        _nodes = nodes;
        _difficulty = difficulty;
        _consensus = consensus;
        _incentives = incentives;
        _scheduler = scheduler;
        _statistics = statistics;
    }

    public void Simulate()
    {
        _difficulty.UpdateDifficultyDecreaseParticipation();
        
        ScheduleInitialEvents();
        RunSimulation();
        
        _consensus.ResolveForks();
        _incentives.DistributeRewards();
        _statistics.Calculate(_nodes);
    }

    private void ScheduleInitialEvents()
    {
        foreach (var node in _nodes)
        {
            _scheduler.ScheduleMineBlockEvent(null, node);
        }
    }

    private void RunSimulation()
    {
        while (_eventPool.HasEvents())
        {
            var @event = _eventPool.NextEvent();
            
            if (@event.EventTime >= _configuration.Simulation.LengthInSeconds)
                return;
           
            @event.Handle();
        }
    }

 
}