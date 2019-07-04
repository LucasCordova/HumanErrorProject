using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HumanErrorProject.Data.Models
{
    public class PreAssignmentCompileFailureReport : PreAssignmentReport
    {
        public PreAssignmentCompileFailureReport()
        {
            Type = PreAssignmentReportTypes.CompileFailure;
        }

        [Required, DisplayName("Compile Failure")]
        public string Report { get; set; }
    }
}
