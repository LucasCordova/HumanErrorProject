using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanErrorProject.Ui.DataVisuals
{
    public class BoxAndWhiskerChart
    {
        public string Id { get; set; }
        public IList<int> Values { get; set; } = new List<int>();
        public string Color { get; set; }
    }
}
