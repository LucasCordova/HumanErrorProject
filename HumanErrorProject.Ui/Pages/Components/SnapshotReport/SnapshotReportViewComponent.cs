using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HumanErrorProject.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace HumanErrorProject.Ui.Pages.Components.SnapshotReport
{
    public class SnapshotReportViewComponent : ViewComponent
    {
        public SnapshotReportViewComponent()
        {
            
        }

        public IViewComponentResult Invoke(Snapshot snapshot)
        {
            switch (snapshot.Report.Type)
            {
                case Data.Models.SnapshotReport.SnapshotReportTypes.Success:
                    return View("SnapshotSuccessReport", snapshot);
                case Data.Models.SnapshotReport.SnapshotReportTypes.Failure:
                    return View("SnapshotFailureReport", snapshot);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

    }
}
