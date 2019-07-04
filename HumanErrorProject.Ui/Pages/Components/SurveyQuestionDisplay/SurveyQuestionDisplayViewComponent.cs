using System;
using HumanErrorProject.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace HumanErrorProject.Ui.Pages.Components.SurveyQuestionDisplay
{
    public class SurveyQuestionDisplayViewComponent : ViewComponent
    {
        public SurveyQuestionDisplayViewComponent()
        {
            
        }

        public IViewComponentResult Invoke(SurveyQuestion question)
        {
            switch (question.Type)
            {
                case SurveyQuestion.SurveyQuestionTypes.Qualitative:
                    return View("SurveyQuestionDisplayQualitative", (SurveyQuestionQualitative) question);
                case HumanErrorProject.Data.Models.SurveyQuestion.SurveyQuestionTypes.Rate:
                    return View("SurveyQuestionDisplayRate", (SurveyQuestionRate)question);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
