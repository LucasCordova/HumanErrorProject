using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HumanErrorProject.Engine.Options
{
    public class MarkovModelOptions
    {
        [DisplayName("Number of States"), Required]
        public int NumberOfStates { get; set; }
        [DisplayName("Weight of Successful Built"), Required]
        public double BuildWeight { get; set; } = 1.0d;
        [DisplayName("Weight of Unit Tests"), Required]
        public double TestWeight { get; set; } = 1.0d;
        [DisplayName("Weight of Bag of Words"), Required]
        public double BagOfWordsWeight { get; set; } = 1.0d;
        [DisplayName("Weight of Abstract Syntax Tree"), Required]
        public double AbstractSyntaxTreeWeight { get; set; } = 1.0d;
        [DisplayName("Build Only Snapshots?"), Required]
        public bool BuildOnly { get; set; }
    }
}
