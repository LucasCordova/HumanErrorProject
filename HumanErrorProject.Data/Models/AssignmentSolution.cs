using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HumanErrorProject.Data.Models
{
    public class AssignmentSolution : IdentityModel<int>
    {
        [Required, DisplayName("Class Name"), StringLength(256, ErrorMessage = "Must be less than 256 characters.")]
        public string Name { get; set; }
        [DisplayName("Solution Files")]
        public virtual byte[] Files { get; set; }
        [DisplayName("Method Declaractions")]
        public virtual IList<MethodDeclaration> MethodDeclarations { get; set; } = new List<MethodDeclaration>();
    }
}
