namespace HumanErrorProject.Engine.Utilities.Filter.ClangCriteria
{
    public class ClangCriteriaHexNode : ClangCriteriaNode
    {
        public override bool Pass(string value)
        {
            return value.StartsWith("0x");
        }
    }
}
