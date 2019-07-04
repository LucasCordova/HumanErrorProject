using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HumanErrorProject.Data.Models
{
    public class SurveyResponse : IdentityModel<int>
    {
        [Required, DisplayName("Question")]
        public int SurveyQuestionId { get; set; }
        [ForeignKey("SurveyQuestionId")]
        public virtual SurveyQuestion Question { get; set; }
        [Required, DisplayName("Answer")]
        public int SurveyAnswerId { get; set; }
        [ForeignKey("SurveyAnswerId")]
        public virtual SurveyAnswer Answer { get; set; }
    }
}
