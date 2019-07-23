using HumanErrorProject.Data.Models;
using HumanErrorProject.Engine.Analysis.AbstractSyntaxTree;
using HumanErrorProject.Engine.Data;
using HumanErrorProject.Engine.Utilities;

namespace HumanErrorProject.Engine.Generators
{
    public interface IAbstractSyntaxTreeGenerator
    {
        AbstractSyntaxTreeNode CreateFromFile(SubmissionData data, string path);
        AbstractSyntaxTreeNode CreateFromFile(DirectoryHandler handler, string path);
        AbstractSyntaxTreeNode CreateOrDefaultFromFile(DirectoryHandler handler, string path);
    }
}
