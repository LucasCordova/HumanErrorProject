using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace HumanErrorProject.Data.Models
{
    public class Student : IdentityModel<string>
    {
        [Required, DisplayName("Student's Full Name"), StringLength(256, ErrorMessage = "Must be less than 256 characters.")]
        public string Name { get; set; }
        [Required, DisplayName("Student's Email"), EmailAddress]
        public string Email { get; set; }
        [DisplayName("Student's Classes")]
        public virtual ICollection<StudentCourseClass> StudentCourseClasses { get; set; } = new List<StudentCourseClass>();
        [DisplayName("Snapshots")]
        public virtual ICollection<Snapshot> Snapshots { get; set; } = new List<Snapshot>();
        [DisplayName("Surveys")]
        public virtual ICollection<Survey> Surveys { get; set; } = new List<Survey>();
        [DisplayName("Submission")]
        public virtual ICollection<SnapshotSubmission> Submissions { get; set; } = new List<SnapshotSubmission>();
    }
}
