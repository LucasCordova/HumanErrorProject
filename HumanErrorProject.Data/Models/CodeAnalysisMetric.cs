using System.ComponentModel.DataAnnotations;

namespace HumanErrorProject.Data.Models
{
    public class CodeAnalysisMetric : IdentityModel<int>
    {
        [Required]
        public int BagOfWordsMertricId { get; set; }
        public virtual BagOfWordsMetric BagOfWordsMetric { get; set; }
        [Required]
        public int AbstractSyntaxTreeMetricId { get; set; }
        public virtual AbstractSyntaxTreeMetric AbstractSyntaxTreeMetric { get; set; }

    }
}
