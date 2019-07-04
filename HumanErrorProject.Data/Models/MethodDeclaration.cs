using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HumanErrorProject.Data.Models
{
    public class MethodDeclaration : IdentityModel<int>
    {
        [Required, DisplayName("Preprocessor Directive"), StringLength(256, ErrorMessage = "Must be less than 256 characters.")]
        public string PreprocessorDirective { get; set; }
        [Required, DisplayName("Ast Type"), StringLength(256, ErrorMessage = "Must be less than 256 characters.")]
        public string AstType { get; set; }
        [Required, DisplayName("Method's Regex Expression"), MinLength(1)]
        public string AstMethodRegexExpression { get; set; }
        [Required, DisplayName("Method's Parameters Regex Expression"), MinLength(1)]
        public string AstMethodParameterRegexExpression { get; set; }
    }
}
