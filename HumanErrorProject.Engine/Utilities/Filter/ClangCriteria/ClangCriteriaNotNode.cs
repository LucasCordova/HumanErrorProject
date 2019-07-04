namespace HumanErrorProject.Engine.Utilities.Filter.ClangCriteria
{
    public class ClangCriteriaNotNode : ClangCriteriaNode
    {
        protected ClangCriteriaNode Node;

        public ClangCriteriaNotNode(ClangCriteriaNode node)
        {
            Node = node;
        }

        public override bool Pass(string value)
        {
            return !Node.Pass(value);
        }
    }
}
