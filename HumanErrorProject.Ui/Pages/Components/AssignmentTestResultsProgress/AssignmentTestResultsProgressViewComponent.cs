using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HumanErrorProject.Data.Models;
using HumanErrorProject.Ui.Constants;
using HumanErrorProject.Ui.DataVisuals;
using Microsoft.AspNetCore.Mvc;

namespace HumanErrorProject.Ui.Pages.Components.AssignmentTestResultsProgress
{
    public class AssignmentTestResultsProgressViewComponent : ViewComponent
    {
        public AssignmentTestResultsProgressViewComponent()
        {
            
        }

        public IViewComponentResult Invoke(IList<Snapshot> snapshots)
        {
            return View("Default", GetLineChart(snapshots));
        }

        public LineChart GetLineChart(IList<Snapshot> snapshots)
        {
            var tests = snapshots.OrderBy(x => x.SnapshotSubmission.CreatedDateTime)
                .Select(NumberOfPassedOrDefault).ToList();
            
            return new LineChart()
            {
                Id = "test_results_progress",
                YValues = tests,
                XValues = tests.Select((x, iter) => iter + 1).ToList(),
                Colors = tests.Select(GetColorOrDefault).ToList(),
            };
        }

        public int NumberOfPassedOrDefault(Snapshot snapshot)
        {
            if (snapshot.Report.Type == Data.Models.SnapshotReport.SnapshotReportTypes.Failure)
                return 0;
            var report = (SnapshotSuccessReport) snapshot.Report;
            return report.UnitTestResults.Count(x => x.Passed);
        }

        public string GetColorOrDefault(int number)
        {
            return number == 0 ? DataVisualConstants.FailedColor : DataVisualConstants.PassedColor;
        }
    }
}
