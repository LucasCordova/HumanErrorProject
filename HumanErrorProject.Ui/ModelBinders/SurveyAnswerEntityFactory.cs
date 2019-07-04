using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HumanErrorProject.Data.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HumanErrorProject.Ui.ModelBinders
{
    public class SurveyAnswerEntityFactory : GenericEntityBinderFactory<SurveyAnswer>
    {
        public override SurveyAnswer Create(string model, IValueProvider valueProvider)
        {
            var id = FirstOrDefault<int>($"{model}.Id", valueProvider);
            var type = Enum.Parse<SurveyAnswer.SurveyAnswerTypes>(
                FirstOrDefault<string>($"{model}.Type", valueProvider));

            switch (type)
            {
                case SurveyAnswer.SurveyAnswerTypes.Qualitative:
                    return new SurveyAnswerQualitative()
                    {
                        Id = id,
                        Type = type,
                        Response = FirstOrDefault<string>($"{model}.Response", valueProvider)
                    };
                case SurveyAnswer.SurveyAnswerTypes.Rate:
                    return new SurveyAnswerRate()
                    {
                        Id = id,
                        Type = type,
                        Selection = FirstOrDefault<int>($"{model}.Selection", valueProvider)
                    };
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
