namespace HumanErrorProject.Engine.Utilities.Filter.ClangCriteria
{
    public class ClangCriteriaColumnNode : ClangCriteriaNode
    {
        public override bool Pass(string value)
        {
            return value.StartsWith("col:");
        }
    }
}
