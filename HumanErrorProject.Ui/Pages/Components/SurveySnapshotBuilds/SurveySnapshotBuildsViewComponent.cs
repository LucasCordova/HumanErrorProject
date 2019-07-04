using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HumanErrorProject.Data.Models;
using HumanErrorProject.Ui.DataVisuals;
using Microsoft.AspNetCore.Mvc;

namespace HumanErrorProject.Ui.Pages.Components.SurveySnapshotBuilds
{
    public class SurveySnapshotBuildsViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(Survey survey)
        {
            return View("Default", GetDonutChart(survey));
        }

        public DonutChart GetDonutChart(Survey survey)
        {
            var success = survey.Snapshots.Count(x =>
                x.Report.Type == Data.Models.SnapshotReport.SnapshotReportTypes.Success);
            var failure = survey.Snapshots.Count(x =>
                x.Report.Type == Data.Models.SnapshotReport.SnapshotReportTypes.Failure);
            return new DonutChart()
            {
                Id = "snapshot_overall_builds",
                Colors = new List<string>()
                {
                    Constants.DataVisualConstants.PassedColor,
                    Constants.DataVisualConstants.FailedColor,
                },
                Values = new List<int>()
                {
                    success,
                    failure,
                },
                Text = $"{Math.Floor((double)success/(success + failure)*100):F0}%"
            };
        }
    }
}
