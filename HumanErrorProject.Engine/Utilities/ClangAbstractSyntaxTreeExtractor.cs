using System;
using System.IO;
using HumanErrorProject.Engine.Analysis.AbstractSyntaxTree;

namespace HumanErrorProject.Engine.Utilities
{
    public class ClangAbstractSyntaxTreeExtractor : IAbstractSyntaxTreeExtractor
    {
        public AbstractSyntaxTreeNode Extract(StreamReader reader)
        {
            var root = GetRoot(reader);

            if (reader.EndOfStream) return root;

            var level = new ClangAbstractSyntaxTreeLevelExtractor(reader,
                reader.ReadLine(), 1);
            level.ExtractLevel(ref root, out _);

            return root;
        }

        public AbstractSyntaxTreeNode GetRoot(StreamReader reader)
        {
            if (reader.EndOfStream)
                throw new ArgumentException($"Unable to make abstract syntax tree node.");
            return new AbstractSyntaxTreeNode(reader.ReadLine());
        }

        public class ClangAbstractSyntaxTreeLevelExtractor
        {
            protected StreamReader Reader;
            protected string CurrentLine;
            protected int Depth;

            public ClangAbstractSyntaxTreeLevelExtractor(StreamReader reader, string currentLine, int depth)
            {
                Reader = reader;
                CurrentLine = currentLine;
                Depth = depth;
            }

            public void ExtractLevel(ref AbstractSyntaxTreeNode root, out string lastLine)
            {
                while (CurrentLine.Length > Depth && CurrentLine[Depth] == '-')
                {
                    var line = CurrentLine.Substring(Depth + 1);
                    var node = new AbstractSyntaxTreeNode(line);

                    if (Reader.EndOfStream)
                    {
                        lastLine = CurrentLine;
                        root.Append(node);
                        return;
                    }

                    var level = new ClangAbstractSyntaxTreeLevelExtractor(Reader, Reader.ReadLine(), Depth + 2);
                    level.ExtractLevel(ref node, out lastLine);
                    CurrentLine = lastLine;
                    root.Append(node);
                }
                lastLine = CurrentLine;
            }
        }
    }
}
