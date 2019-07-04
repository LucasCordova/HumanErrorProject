using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HumanErrorProject.Data.Models
{
    public abstract class SurveyAnswer : IdentityModel<int>
    {
        [Required, DisplayName("Answer Type")]
        public SurveyAnswerTypes Type { get; set; }

        public enum SurveyAnswerTypes
        {
            Qualitative,
            Rate
        }
    }
}
