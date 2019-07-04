using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HumanErrorProject.Data.Models;
using HumanErrorProject.Ui.DataVisuals;
using Microsoft.AspNetCore.Mvc;

namespace HumanErrorProject.Ui.Pages.Components.AssignmentSnapshotsTestResults
{
    public class AssignmentSnapshotsTestResultsViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(IList<Snapshot> snapshots)
        {
            return View("Default", GetBarChart(snapshots));
        }

        public BarChart GetBarChart(IList<Snapshot> snapshots)
        {
            return new BarChart()
            {
                Id = "overall_test_results",
                Minimum = 0,
                Values = new List<int>()
                {
                    snapshots.Select(x => ((SnapshotSuccessReport)x.Report).UnitTestResults.Count(y => y.Passed)).Min(),
                    snapshots.Select(x => ((SnapshotSuccessReport)x.Report).UnitTestResults.Count(y => y.Passed)).Max(),
                    (int)snapshots.Select(x => ((SnapshotSuccessReport)x.Report).UnitTestResults.Count(y => y.Passed)).Average(),
                },
                Colors = new List<string>()
                {
                    Constants.DataVisualConstants.FailedColor,
                    Constants.DataVisualConstants.PassedColor,
                    Constants.DataVisualConstants.PrimaryColor,
                },
                Labels = new List<string>()
                {
                    "Worst",
                    "Best",
                    "Average"
                }
            };
        }
    }
}
