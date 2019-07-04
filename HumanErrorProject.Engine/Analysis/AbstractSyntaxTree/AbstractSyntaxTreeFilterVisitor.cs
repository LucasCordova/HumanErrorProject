using System.Linq;
using HumanErrorProject.Engine.Utilities.Filter;

namespace HumanErrorProject.Engine.Analysis.AbstractSyntaxTree
{
    public class AbstractSyntaxTreeFilterVisitor : IAbstractSyntaxTreeVisitor
    {
        protected ILineFilter Filter;
        public AbstractSyntaxTreeFilterVisitor(ILineFilter filter)
        {
            Filter = filter;
        }

        public void Visit(AbstractSyntaxTreeNode node)
        {
            node.Value = Filter.Filter(node.Value);
        }
    }
}
