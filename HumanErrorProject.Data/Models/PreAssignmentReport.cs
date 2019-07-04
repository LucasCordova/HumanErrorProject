using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HumanErrorProject.Data.Models
{
    public abstract class PreAssignmentReport : IdentityModel<int>
    {
        [Required, DisplayName("Report Type")]
        public PreAssignmentReportTypes Type { get; set; }

        public enum PreAssignmentReportTypes
        {
            Pending,
            Success,
            NoFileFailure,
            CompileFailure,
            BuildFailure,
            NoClassFailure,
            MissingMethodsFailure,
            FailTestsFailure,
            BadTestFolder,
        }

        public string TypeValue()
        {
            switch (Type)
            {
                case PreAssignmentReportTypes.Pending:
                    return "Pending";
                case PreAssignmentReportTypes.Success:
                    return "Success";
                case PreAssignmentReportTypes.NoFileFailure:
                    return "No File Found";
                case PreAssignmentReportTypes.CompileFailure:
                    return "Compile Failure";
                case PreAssignmentReportTypes.BuildFailure:
                    return "Build Failure";
                case PreAssignmentReportTypes.NoClassFailure:
                    return "No Class Found";
                case PreAssignmentReportTypes.MissingMethodsFailure:
                    return "Missing Methods";
                case PreAssignmentReportTypes.FailTestsFailure:
                    return "Fail Unit Tests";
                case PreAssignmentReportTypes.BadTestFolder:
                    return "Incorrect Test Folder";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
