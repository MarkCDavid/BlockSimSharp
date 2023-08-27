using BlockSimSharp.BitcoinBurn.Simulation.Data;
using BlockSimSharp.BitcoinBurn.SimulationConfiguration;

namespace BlockSimSharp.BitcoinBurn.SimulationStatistics.Data;

public class Blocks
{
    
    public int Total { get; set; }
    public int Main { get; set; }
    public int Stale { get; set; }
    public double StaleRate { get; set; }
    public List<Model.Block> Chain { get; } = new();

    public void Update(GlobalBlockChain globalBlockChain, Configuration configuration)
    {
        Main = globalBlockChain.Count - 1;
        Stale = Total - Main;
        StaleRate = Math.Round((double)Stale / Total, 2);

        if (configuration.Statistics.IncludeGlobalBlockChain)
        {
            foreach (var globalBlock in globalBlockChain.OrderBy(block => block.Depth))
            {
                var block = new Model.Block(globalBlock);
                Chain.Add(block);
            }
        }
    }
}