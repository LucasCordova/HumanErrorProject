using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HumanErrorProject.Data.Models
{
    public abstract class SnapshotReport : IdentityModel<int>
    {
        [Required, DisplayName("Report Type")]
        public SnapshotReportTypes Type { get; set; }

        public enum SnapshotReportTypes
        {
            Success,
            Failure
        }

        public string TypeValue()
        {
            switch (Type)
            {
                case SnapshotReportTypes.Success:
                    return "Success";
                case SnapshotReportTypes.Failure:
                    return "Failure";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
