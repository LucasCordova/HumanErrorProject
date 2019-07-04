using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Internal;

namespace HumanErrorProject.Engine.Utilities.Filter.ClangCriteria
{
    public class ClangCriteriaAndNode : ClangCriteriaNode
    {
        protected IList<ClangCriteriaNode> Nodes;
        public ClangCriteriaAndNode(IList<ClangCriteriaNode> nodes)
        {
            Nodes = nodes;
        }

        public override bool Pass(string value)
        {
            var node = Nodes.FirstOrDefault(n => !n.Pass(value));
            return node == null;
        }
    }
}
