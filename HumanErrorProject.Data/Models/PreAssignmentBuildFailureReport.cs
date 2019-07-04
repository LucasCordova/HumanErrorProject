using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HumanErrorProject.Data.Models
{
    public class PreAssignmentBuildFailureReport : PreAssignmentReport
    {
        public PreAssignmentBuildFailureReport()
        {
            Type = PreAssignmentReportTypes.BuildFailure;
        }

        [Required, DisplayName("Build Failure")]
        public string Report { get; set; }
    }
}
