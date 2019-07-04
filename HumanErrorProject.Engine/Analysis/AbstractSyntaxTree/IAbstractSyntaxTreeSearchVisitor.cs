namespace HumanErrorProject.Engine.Analysis.AbstractSyntaxTree
{
    public interface IAbstractSyntaxTreeSearchVisitor
    {
        bool Found { get; }
        void Visit(AbstractSyntaxTreeNode node);
    }
}
