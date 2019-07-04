using System.Collections.Generic;
using System.ComponentModel;

namespace HumanErrorProject.Data.Models
{
    public class PreAssignmentFailTestsFailureReport : PreAssignmentReport
    {
        public PreAssignmentFailTestsFailureReport()
        {
            Type = PreAssignmentReportTypes.FailTestsFailure;
        }

        [DisplayName("Fail Unit Tests")]
        public virtual ICollection<UnitTest> FailUnitTests { get; set; } = new List<UnitTest>();
    }
}
