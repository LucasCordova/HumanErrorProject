using HumanErrorProject.Data.Models;
using HumanErrorProject.Engine.Analysis.AbstractSyntaxTree;

namespace HumanErrorProject.Engine.Utilities
{
    public interface IAbstractSyntaxTreeMethodExtractor
    {
        AbstractSyntaxTreeNode ExtractOrDefault(AbstractSyntaxTreeNode root, MethodDeclaration methodDeclaration);
    }
}
