using System;
using HumanErrorProject.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace HumanErrorProject.Ui.Pages.Components.PreAssignmentReport
{
    public class PreAssignmentReportViewComponent : ViewComponent
    {
        public PreAssignmentReportViewComponent()
        {
            
        }

        public IViewComponentResult Invoke(PreAssignment assignment)
        {
            switch (assignment.PreAssignmentReport.Type)
            {
                case HumanErrorProject.Data.Models.PreAssignmentReport.PreAssignmentReportTypes.Pending:
                    return View("PreAssignmentPendingReport", assignment);
                case HumanErrorProject.Data.Models.PreAssignmentReport.PreAssignmentReportTypes.Success:
                    return View("PreAssignmentSuccessReport", assignment);
                case HumanErrorProject.Data.Models.PreAssignmentReport.PreAssignmentReportTypes.NoFileFailure:
                    return View("PreAssignmentNoFileFailureReport", assignment);
                case HumanErrorProject.Data.Models.PreAssignmentReport.PreAssignmentReportTypes.CompileFailure:
                    return View("PreAssignmentCompileFailureReport",  assignment);
                case HumanErrorProject.Data.Models.PreAssignmentReport.PreAssignmentReportTypes.BuildFailure:
                    return View("PreAssignmentBuildFailureReport",  assignment);
                case HumanErrorProject.Data.Models.PreAssignmentReport.PreAssignmentReportTypes.NoClassFailure:
                    return View("PreAssignmentNoClassFailureReport",  assignment);
                case HumanErrorProject.Data.Models.PreAssignmentReport.PreAssignmentReportTypes.MissingMethodsFailure:
                    return View("PreAssignmentMissingMethodsFailureReport",  assignment);
                case HumanErrorProject.Data.Models.PreAssignmentReport.PreAssignmentReportTypes.FailTestsFailure:
                    return View("PreAssignmentFailTestsFailureReport",  assignment);
                case HumanErrorProject.Data.Models.PreAssignmentReport.PreAssignmentReportTypes.BadTestFolder:
                    return View("PreAssignmentBadTestFolderReport", assignment);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
