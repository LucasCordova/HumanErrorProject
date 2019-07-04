using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HumanErrorProject.Data.Models;
using HumanErrorProject.Ui.DataVisuals;
using Microsoft.AspNetCore.Mvc;

namespace HumanErrorProject.Ui.Pages.Components.AssignmentSnapshotsBuilds
{
    public class AssignmentSnapshotsBuildsViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(Assignment assignment)
        {
            return View("Default", GetDonutChart(assignment));
        }

        public DonutChart GetDonutChart(Assignment assignment)
        {
            var success = assignment.Snapshots.Count(x =>
                x.Report.Type == Data.Models.SnapshotReport.SnapshotReportTypes.Success);
            var failure = assignment.Snapshots.Count(x =>
                x.Report.Type == Data.Models.SnapshotReport.SnapshotReportTypes.Failure);
            return new DonutChart()
            {
                Id = "assignment_overall_builds",
                Colors = new List<string>()
                {
                    Constants.DataVisualConstants.PassedColor,
                    Constants.DataVisualConstants.FailedColor
                },
                Values = new List<int>()
                {
                    success,
                    failure
                },
                Text = $"{Math.Floor((double)success / (success + failure) * 100):F0}%"
            };
        }
    }
}
