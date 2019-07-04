using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanErrorProject.Ui.DataVisuals
{
    public class DonutChart
    {
        public string Id { get; set; }
        public IList<int> Values { get; set; } = new List<int>();
        public IList<string> Colors { get; set; } = new List<string>();
        public string Text { get; set; }
    }
}
