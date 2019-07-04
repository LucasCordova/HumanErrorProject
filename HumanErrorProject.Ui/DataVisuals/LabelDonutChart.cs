using System.Collections.Generic;

namespace HumanErrorProject.Ui.DataVisuals
{
    public class LabelDonutChart 
    {
        public string Id { get; set; }
        public IList<int> Values { get; set; } = new List<int>();
        public IList<string> Colors { get; set; } = new List<string>();
        public IList<string> Labels { get; set; } = new List<string>();
    }
}
