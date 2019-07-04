using HumanErrorProject.Data.Models;
using HumanErrorProject.Engine.Analysis.AbstractSyntaxTree;

namespace HumanErrorProject.Engine.Generators
{
    public interface ISnapshotMethodGenerator
    {
        SnapshotMethod Generate(AbstractSyntaxTreeNode student, AbstractSyntaxTreeNode solution,
            MethodDeclaration method);
    }
}
