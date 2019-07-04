using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HumanErrorProject.Data.Models
{
    public class SnapshotFailureReport : SnapshotReport
    {
        public SnapshotFailureReport()
        {
            Type = SnapshotReportTypes.Failure;
        }

        [Required, DisplayName("Failure Report")]
        public string Report { get; set; }
    }
}
