using System;
using System.Collections.Generic;
using System.Linq;
using HumanErrorProject.Data.Models;
using HumanErrorProject.Ui.DataVisuals;
using Microsoft.AspNetCore.Mvc;

namespace HumanErrorProject.Ui.Pages.Components.SnapshotTestResults
{
    public class SnapshotTestResultsViewComponent : ViewComponent
    {
        public SnapshotTestResultsViewComponent()
        {
        }

        public IViewComponentResult Invoke(Snapshot snapshot)
        {
            return View("Default", GetLabelDonutChart(snapshot));
        }

        public LabelDonutChart GetLabelDonutChart(Snapshot snapshot)
        {
            var report = (SnapshotSuccessReport) snapshot.Report;

            var categories = report
                .UnitTestResults.Select(x => x.UnitTest.Category)
                .Distinct();

            var labels = new List<string>()
            {
                "Passed Tests"
            };
            var colors = new List<string>()
            {
                Constants.DataVisualConstants.PassedColor
            };
            var domain = new List<int>()
            {
                report.UnitTestResults.Count(x => x.Passed),
            };


            foreach (var category in categories)
            {
                var count = report.UnitTestResults
                    .Count(x => !x.Passed && x.UnitTest.Category.Equals(category));
                colors.Add(Constants.DataVisualConstants.FailedColor);
                domain.Add(count);
                labels.Add(category);
            }

            var i = 0;
            while (i < labels.Count)
            {
                if (domain[i] == 0)
                {
                    labels.RemoveAt(i);
                    colors.RemoveAt(i);
                    domain.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }

            return new LabelDonutChart()
            {
                Id = "test_results",
                Values = domain,
                Colors = colors,
                Labels = labels,
            };
        }
    }
}
