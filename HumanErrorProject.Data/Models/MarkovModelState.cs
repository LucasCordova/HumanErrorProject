using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HumanErrorProject.Data.Models
{
    public class MarkovModelState : IdentityModel<int>
    {
        [Required, DisplayName("State's Number")]
        public int Number { get; set; }
        public virtual ICollection<MarkovModelSnapshot> Snapshots { get; set; } = new List<MarkovModelSnapshot>();

        [DisplayName("State's Transitions")]
        public virtual ICollection<MarkovModelTransition> Transitions { get; set; } = new List<MarkovModelTransition>();

        public string AnchorTag => $"state_{Number}";
    }
}
