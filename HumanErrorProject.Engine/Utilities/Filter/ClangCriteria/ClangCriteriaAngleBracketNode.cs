namespace HumanErrorProject.Engine.Utilities.Filter.ClangCriteria
{
    public class ClangCriteriaAngleBracketNode : ClangCriteriaNode
    {
        public override bool Pass(string value)
        {
            return value.StartsWith('<') && value.EndsWith('>');
        }
    }
}
