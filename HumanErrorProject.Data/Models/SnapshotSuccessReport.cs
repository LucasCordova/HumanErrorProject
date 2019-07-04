using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HumanErrorProject.Data.Models
{
    public class SnapshotSuccessReport : SnapshotReport
    {
        public SnapshotSuccessReport()
        {
            Type = SnapshotReportTypes.Success;
        }

        [Required, DisplayName("Snapshot's Methods")]
        public virtual ICollection<SnapshotMethod> SnapshotMethods { get; set; } = new List<SnapshotMethod>();
        [Required, DisplayName("Snapshot's Test Results")]
        public virtual ICollection<UnitTestResult> UnitTestResults { get; set; } = new List<UnitTestResult>();
    }
}
