namespace HumanErrorProject.Engine.Analysis.AbstractSyntaxTree
{
    public interface IAbstractSyntaxTreeVisitor
    {
        void Visit(AbstractSyntaxTreeNode node);
    }
}
