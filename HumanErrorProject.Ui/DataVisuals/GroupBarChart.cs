using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanErrorProject.Ui.DataVisuals
{
    public class GroupBarChart
    {
        public string Id { get; set; }
        public IList<IList<int>> Values { get; set; } = new List<IList<int>>();
        public IList<string> Colors { get; set; } = new List<string>();
        public IList<string> SubGroups { get; set; } = new List<string>();
        public IList<string> Groups { get; set; } = new List<string>();
        public int Minimum { get; set; } = 0;
        public int Maximum { get; set; } = 0;
    }
}
