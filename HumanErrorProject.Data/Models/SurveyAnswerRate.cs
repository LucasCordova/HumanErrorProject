using System.ComponentModel;

namespace HumanErrorProject.Data.Models
{
    public class SurveyAnswerRate : SurveyAnswer
    {
        public SurveyAnswerRate()
        {
            Type = SurveyAnswerTypes.Rate;
        }

        [DisplayName("Survey's Selection")]
        public int Selection { get; set; }
    }
}
