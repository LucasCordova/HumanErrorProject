using System.Collections.Generic;
using System.Linq;
using HumanErrorProject.Engine.Utilities.Filter.ClangCriteria;
using Microsoft.EntityFrameworkCore.Internal;

namespace HumanErrorProject.Engine.Utilities.Filter
{
    public class ClangLineFilter : ILineFilter
    {
        protected ClangCriteriaNode Node;
        protected ILineSplitter Splitter;
        public ClangLineFilter(ClangCriteriaNode node, ILineSplitter splitter)
        {
            Node = node;
            Splitter = splitter;
        }

        public ClangLineFilter(ILineSplitter splitter)
        {
            Node = new ClangCriteriaAndNode(
                new List<ClangCriteriaNode>()
                {
                    new ClangCriteriaNotNode(new ClangCriteriaHexNode()),
                    new ClangCriteriaNotNode(new ClangCriteriaAngleBracketNode()),
                    new ClangCriteriaNotNode(new ClangCriteriaConstantsNode()),
                    new ClangCriteriaNotNode(new ClangCriteriaColumnNode()),
                });
            Splitter = splitter;
        }

        public string Filter(string value)
        {
            var values = Splitter.Split(value);
            return values.Where(v => Node.Pass(v)).Join(" ");
        }
    }
}
