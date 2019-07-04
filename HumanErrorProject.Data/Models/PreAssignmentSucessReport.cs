using System.Collections.Generic;

namespace HumanErrorProject.Data.Models
{
    public class PreAssignmentSucessReport : PreAssignmentReport
    {
        public PreAssignmentSucessReport()
        {
            Type = PreAssignmentReportTypes.Success;
        }
    }
}
