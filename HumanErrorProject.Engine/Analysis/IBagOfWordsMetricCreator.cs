using HumanErrorProject.Data.Models;
using HumanErrorProject.Engine.Analysis.AbstractSyntaxTree;

namespace HumanErrorProject.Engine.Analysis
{
    public interface IBagOfWordsMetricCreator
    {
        BagOfWordsMetric Create(AbstractSyntaxTreeNode expected, AbstractSyntaxTreeNode actual);
    }
}
