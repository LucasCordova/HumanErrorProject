using System.IO;
using HumanErrorProject.Engine.Analysis.AbstractSyntaxTree;

namespace HumanErrorProject.Engine.Utilities
{
    public interface IAbstractSyntaxTreeExtractor
    {
        AbstractSyntaxTreeNode Extract(StreamReader reader);
    }
}
