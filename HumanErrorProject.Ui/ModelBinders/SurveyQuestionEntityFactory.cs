using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HumanErrorProject.Data.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HumanErrorProject.Ui.ModelBinders
{
    public class SurveyQuestionEntityFactory : GenericEntityBinderFactory<SurveyQuestion>
    {
        public override SurveyQuestion Create(string model, IValueProvider valueProvider)
        {
            var id = FirstOrDefault<int>($"{model}.Id", valueProvider);
            var type = Enum.Parse<SurveyQuestion.SurveyQuestionTypes>(FirstOrDefault<string>($"{model}.Type", valueProvider));

            switch (type)
            {
                case SurveyQuestion.SurveyQuestionTypes.Qualitative:
                    return new SurveyQuestionQualitative()
                    {
                        Id = id,
                        Type = type,
                        Prompt = FirstOrDefault<string>($"{model}.Prompt", valueProvider),
                        Required = FirstOrDefault<bool>($"{model}.Required", valueProvider)
                    };
                case SurveyQuestion.SurveyQuestionTypes.Rate:
                    return new SurveyQuestionRate()
                    {
                        Id = id,
                        Type = type,
                        Category = FirstOrDefault<string>($"{model}.Category", valueProvider),
                        Explaination = FirstOrDefault<string>($"{model}.Explaination", valueProvider),
                        Example = FirstOrDefault<string>($"{model}.Example", valueProvider),
                        Required = FirstOrDefault<bool>($"{model}.Required", valueProvider),
                        Range = FirstOrDefault<int>($"{model}.Range", valueProvider)
                    };
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
