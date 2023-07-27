using BlockSimSharp.Bitcoin.Model;
using BlockSimSharp.Bitcoin.Simulation.Events;
using BlockSimSharp.Core.Simulation;

namespace BlockSimSharp.Bitcoin.Simulation;

public class Incentives: BaseIncentives<Transaction, Block, Node, Network, Consensus, Event, Scheduler, Context>
{

    public override void DistributeRewards(Context context)
    {
        
    }
}