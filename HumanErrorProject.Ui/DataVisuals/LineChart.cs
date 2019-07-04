using System.Collections.Generic;

namespace HumanErrorProject.Ui.DataVisuals
{
    public class LineChart
    {
        public string Id { get; set; }
        public IList<int> XValues { get; set; } = new List<int>();
        public IList<int> YValues { get; set; } = new List<int>();
        public IList<string> Colors { get; set; } = new List<string>();
    }
}
