using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanErrorProject.Ui.DataVisuals
{
    public class BarChart
    {
        public string Id { get; set; }
        public IList<int> Values { get; set; } = new List<int>();
        public IList<string> Colors { get; set; } = new List<string>();
        public IList<string> Labels { get; set; } = new List<string>();
        public int Minimum { get; set; } = 0;
    }
}
