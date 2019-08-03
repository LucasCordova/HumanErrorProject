using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanErrorProject.Ui.DataVisuals
{
    public class GraphChart
    {
        public string Id { get; set; }
        public IList<GraphChartNode> GraphChartNodes { get; set; } = new List<GraphChartNode>();
        public IList<GraphCartLink> GraphCartLinks { get; set; } = new List<GraphCartLink>();  
    }
}
