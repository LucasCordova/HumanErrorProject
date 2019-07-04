using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HumanErrorProject.Data.Models
{
    public class Assignment : IdentityModel<int>
    {
        [Required, DisplayName("Assignment's Name"), StringLength(256, ErrorMessage = "Must be less than 256 characters.")]
        public string Name { get; set; }
        [Required, DisplayName("Assignment's File Entry Point"), StringLength(256, ErrorMessage = "Must be less than 256 characters.")]
        public string Filename { get; set; }
        [Required, DisplayName("Class")]
        public int CourseClassId { get; set; }
        public virtual CourseClass CourseClass { get; set; }
        [Required, DisplayName("Solution")]
        public int AssignmentSolutionId { get; set; }
        public virtual AssignmentSolution Solution { get; set; } = new AssignmentSolution();
        [Required, DisplayName("Test Project")]
        public int TestProjectId { get; set; }
        public virtual TestProject TestProject { get; set; } = new TestProject();
        public virtual ICollection<Snapshot> Snapshots { get; set; } = new List<Snapshot>();
    }
}
