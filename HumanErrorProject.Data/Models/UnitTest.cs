using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace HumanErrorProject.Data.Models
{
    public class UnitTest : IdentityModel<int>
    {
        [Required, DisplayName("Unit Test's Name"), MinLength(1)]
        public string Name { get; set; }
        public string Category =>
            Regex.IsMatch(Name, "(.+)(__)(.*)") ? Regex.Match(Name, "(.+)(__)(.*)").Groups[1].Value : Name;
    }
}
