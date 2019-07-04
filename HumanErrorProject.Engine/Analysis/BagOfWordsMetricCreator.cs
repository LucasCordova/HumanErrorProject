using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using HumanErrorProject.Data.Models;
using HumanErrorProject.Engine.Analysis.AbstractSyntaxTree;
using HumanErrorProject.Engine.Utilities;

namespace HumanErrorProject.Engine.Analysis
{
    public class BagOfWordsMetricCreator : IBagOfWordsMetricCreator
    {
        protected ILineSplitter Splitter;

        public BagOfWordsMetricCreator(ILineSplitter splitter)
        {
            Splitter = splitter;
        }

        public BagOfWordsMetric Create(AbstractSyntaxTreeNode expected, AbstractSyntaxTreeNode actual)
        {
            var expectedVisitor = new BagOfWordsMetricVisitor(Splitter);
            var actualVisitor = new BagOfWordsMetricVisitor(Splitter);

            expected.PreOrder(expectedVisitor);
            actual.PreOrder(actualVisitor);

            var lexicon = CreateJointLexicon(expectedVisitor.Lexicon, actualVisitor.Lexicon);

            var expectedVector = CreateWordVector(expectedVisitor.Content, lexicon);
            var actualVector = CreateWordVector(actualVisitor.Content, lexicon);

            return new BagOfWordsMetric()
            {
                Difference = CalculateDifference(expectedVector, actualVector)
            };
        }

        public double CalculateDifference(int[] left, int[] right)
        {
            if (left.Length != right.Length) throw new ArgumentException("Length Aren't The Same");
            var value = left.Select((t, i) => Math.Pow(right[i] - t, 2)).Sum();
            return Math.Pow(value, 0.5);
        }

        private IList<string> CreateJointLexicon(IList<string> l1, IList<string> l2)
        {
            foreach (var token in l2)
            {
                if (!l1.Contains(token)) l1.Add(token);
            }
            return l1;
        }

        private int[] CreateWordVector(string source, IList<string> lexicon)
        {
            var wordVector = new int[lexicon.Count];

            for (var i = 0; i < lexicon.Count; i++)
            {
                var amount = Regex.Matches(source, $@"\s({RegexLexicon(lexicon[i])})\s", RegexOptions.IgnoreCase).Count;
                source = Regex.Replace(source, $@"\s({RegexLexicon(lexicon[i])})\s", " ", RegexOptions.IgnoreCase);
                wordVector[i] = amount;
            }
            return wordVector;
        }

        private string RegexLexicon(string word)
        {
            var line = "";
            foreach (var ch in word)
            {
                switch (ch)
                {
                    case '(':
                    case '{':
                    case '}':
                    case ')':
                    case '[':
                    case ']':
                    case '.':
                    case '*':
                    case '^':
                    case '|':
                    case '+':
                        line += @"\";
                        line += ch;
                        break;
                    default:
                        line += ch;
                        break;
                }
            }
            return line;
        }

        private class BagOfWordsMetricVisitor : IAbstractSyntaxTreeVisitor
        {
            private readonly ILineSplitter _splitter;

            public IList<string> Lexicon { get; } = new List<string>();
            public string Content { get; private set; } = "";

            public BagOfWordsMetricVisitor(ILineSplitter splitter)
            {
                _splitter = splitter;
            }

            public void Visit(AbstractSyntaxTreeNode node)
            {
                var words = _splitter.Split(node.Value);
                foreach (var word in words)
                {
                    Content +=  " " + word + " ";
                    if (!Lexicon.Contains(word)) Lexicon.Add(word);
                }
            }
        }
    }
}
