using HumanErrorProject.Engine.Utilities;

namespace HumanErrorProject.Engine.Analysis.AbstractSyntaxTree
{
    public class AbstractSyntaxTreeWordSearchVisitor : IAbstractSyntaxTreeSearchVisitor
    {
        protected ILineSplitter Splitter;
        protected string Word;
        public bool Found { get; private set; }
        public AbstractSyntaxTreeNode Node { get; private set; }
        public AbstractSyntaxTreeWordSearchVisitor(ILineSplitter splitter, string word)
        {
            Splitter = splitter;
            Word = word;
        }

        public void Visit(AbstractSyntaxTreeNode node)
        {
            var values = Splitter.Split(node.Value);
            foreach (var value in values)
            {
                if (value.Equals(Word))
                {
                    Found = true;
                    Node = node;
                }
            }
        }
    }
}
