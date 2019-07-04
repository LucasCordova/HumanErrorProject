using HumanErrorProject.Data.Models;
using HumanErrorProject.Engine.Analysis.AbstractSyntaxTree;

namespace HumanErrorProject.Engine.Analysis
{
    public interface IAbstractSyntaxTreeMetricCreator
    {
        AbstractSyntaxTreeMetric Create(AbstractSyntaxTreeNode expected, AbstractSyntaxTreeNode actual);
    }
}
