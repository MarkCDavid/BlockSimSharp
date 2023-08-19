using BlockSimSharp.BitcoinBurn.Simulation;
using BlockSimSharp.BitcoinBurn.SimulationConfiguration;
using BlockSimSharp.BitcoinBurn.SimulationStatistics;
using Node = BlockSimSharp.BitcoinBurn.Model.Node;

namespace BlockSimSharp.BitcoinBurn;

public class Simulator
{
    private readonly Configuration _configuration;
    private readonly IReadOnlyList<Node> _nodes;
    private readonly Scheduler _scheduler;
    private readonly Consensus _consensus;
    private readonly Incentives _incentives;
    private readonly Statistics _statistics;

    public Simulator(Configuration configuration, IReadOnlyList<Node> nodes, Scheduler scheduler, Consensus consensus, Incentives incentives, Statistics statistics)
    {
        _configuration = configuration;
        _nodes = nodes;
        _scheduler = scheduler;
        _consensus = consensus;
        _incentives = incentives;
        _statistics = statistics;
    }

    public void Simulate()
    {
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
            _scheduler.ScheduleInitialEvents(node);
        }
    }

    private void RunSimulation()
    {
        while (_scheduler.HasEvents())
        {
            var @event = _scheduler.NextEvent();
            
            if (@event.EventTime >= _configuration.Simulation.LengthInSeconds)
                return;
           
            @event.Handle();
        }
    }

 
}