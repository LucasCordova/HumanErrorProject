using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HumanErrorProject.Data.Models
{
    public class Snapshot : IdentityModel<int>
    {
        [Required]
        public int SnapshotSubmissionId { get; set; }
        public virtual SnapshotSubmission SnapshotSubmission { get; set; }
        [Required]
        public int AssignmentId { get; set; }
        public virtual Assignment Assignment { get; set; }
        [Required]
        public string StudentId { get; set; }
        public virtual Student Student { get; set; }
        public string SurveyId { get; set; }
        public virtual Survey Survey { get; set; }
        [Required, DisplayName("Snapshot's Report")]
        public int SnapshotReportId { get; set; } 
        public virtual SnapshotReport Report { get; set; }
    }
}
