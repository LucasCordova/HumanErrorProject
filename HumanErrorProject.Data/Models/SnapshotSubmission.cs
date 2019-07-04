using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HumanErrorProject.Data.Models
{
    public class SnapshotSubmission : IdentityModel<int>
    {
        [Required, DisplayName("Created Date")]
        public DateTime CreatedDateTime { get; set; }
        [Required, DisplayName("Snapshot's Files")]
        public byte[] Files { get; set; }
        public int? StudentId { get; set; }
        public virtual Student Student { get; set; }
    }
}
