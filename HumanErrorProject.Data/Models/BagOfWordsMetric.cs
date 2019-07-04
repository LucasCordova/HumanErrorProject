using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HumanErrorProject.Data.Models
{
    public class BagOfWordsMetric : IdentityModel<int>
    {
        [Required, DisplayName("Difference in Words")]
        public double Difference { get; set; }
    }
}
