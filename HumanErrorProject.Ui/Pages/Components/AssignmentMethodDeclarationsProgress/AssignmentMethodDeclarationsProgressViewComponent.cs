using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HumanErrorProject.Data.Models;
using HumanErrorProject.Ui.Constants;
using HumanErrorProject.Ui.DataVisuals;
using Microsoft.AspNetCore.Mvc;

namespace HumanErrorProject.Ui.Pages.Components.AssignmentMethodDeclarationsProgress
{
    public class AssignmentMethodDeclarationsProgressViewComponent : ViewComponent
    {

        public IViewComponentResult Invoke(IList<Snapshot> snapshots)
        {
            return View("Default", GetLineChart(snapshots));
        }

        public LineChart GetLineChart(IList<Snapshot> snapshots)
        {
            var tests = snapshots.OrderBy(x => x.SnapshotSubmission.CreatedDateTime)
                .Select(NumberOfDeclaredOrDefault).ToList();

            return new LineChart()
            {
                Id = "method_declarations_progress",
                YValues = tests,
                XValues = tests.Select((x, iter) => iter + 1).ToList(),
                Colors = tests.Select(GetColorOrDefault).ToList(),
            };
        }

        public int NumberOfDeclaredOrDefault(Snapshot snapshot)
        {
            if (snapshot.Report.Type == Data.Models.SnapshotReport.SnapshotReportTypes.Failure)
                return 0;
            var report = (SnapshotSuccessReport)snapshot.Report;
            return report.SnapshotMethods.Count(x => x.Declared);
        }

        public string GetColorOrDefault(int number)
        {
            return number == 0 ? DataVisualConstants.FailedColor : DataVisualConstants.PassedColor;
        }
    }
}
