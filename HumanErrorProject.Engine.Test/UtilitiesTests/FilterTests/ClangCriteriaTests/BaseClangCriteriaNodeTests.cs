using HumanErrorProject.Engine.Utilities.Filter.ClangCriteria;

namespace HumanErrorProject.Engine.Test.UtilitiesTests.FilterTests.ClangCriteriaTests
{
    public abstract class BaseClangCriteriaNodeTests
    {
        public class MockClangCriteriaNode : ClangCriteriaNode
        {
            public MockClangCriteriaNode(string word)
            {
                Word = word;
            }
            public string Word { get;}
            public override bool Pass(string value)
            {
                return value.Contains(Word);
            }
        }
    }
}
