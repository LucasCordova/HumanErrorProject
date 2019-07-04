using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HumanErrorProject.Data.Models
{
    public class TestProject : IdentityModel<int>
    {
        [Required, DisplayName("Source Files Location"), MinLength(1)]
        public string TestFolder { get; set; }
        [Required, DisplayName("Project File"), MinLength(1)]
        public string TestProjectFile { get; set; }
        [Required, DisplayName("Generated Dll File"), MinLength(1)]
        public string TestDllFile { get; set; }
        [DisplayName("Test Project Zip Files")]
        public virtual byte[] Files { get; set; }
        [DisplayName("Unit Tests")]
        public virtual ICollection<UnitTest> UnitTests { get; set; } = new List<UnitTest>();
    }
}
