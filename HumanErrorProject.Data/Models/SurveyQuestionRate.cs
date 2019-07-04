using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HumanErrorProject.Data.Models
{
    public class SurveyQuestionRate : SurveyQuestion
    {
        public SurveyQuestionRate()
        {
            Type = SurveyQuestionTypes.Rate;
        }

        [Required, DisplayName("Category")]
        public string Category { get; set; }
        [Required, DisplayName("Rate's Range")]
        public int Range { get; set; }
        [Required, DisplayName("Example")]
        public string Example { get; set; }
        [Required, DisplayName("Explaination of Question")]
        public string Explaination { get; set; }
    }
}
