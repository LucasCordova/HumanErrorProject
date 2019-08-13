using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HumanErrorProject.Data.Models
{
    public class CourseClass : IdentityModel<int>
    {
        [Required, DisplayName("Class Name")]
        public string Name { get; set; }
        [Required, DisplayName("Term")]
        public string Term { get; set; }
        [Required, DisplayName("Course Name")]
        public string Course { get; set; }
        [DisplayName("Class Assignments")]
        public virtual ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();
        [DisplayName("Students")]
        public virtual ICollection<StudentCourseClass> StudentCourseClasses { get; set; } = new List<StudentCourseClass>();
        [DisplayName("In Progress Assignments")]
        public virtual ICollection<PreAssignment> PreAssignments { get; set; } = new List<PreAssignment>();
        [DisplayName("Survey Questions")]
        public virtual ICollection<SurveyQuestion> SurveyQuestions { get; set; } = new List<SurveyQuestion>();
    }
}
