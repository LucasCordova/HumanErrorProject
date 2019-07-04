using System.Collections.Generic;

namespace HumanErrorProject.Engine.Utilities
{
    public class ClangLineSplitter : ILineSplitter
    {
        public string[] Split(string line)
        {
            var index = 0;
            var splitStrings = new List<string>();
            while (index < line.Length)
            {
                if (line[index] == ' ')
                {
                    index++;
                }
                else
                {
                    splitStrings.Add(SplitString(line, index, out index));
                }
            }

            return splitStrings.ToArray();
        }

        private string SplitString(string input, int startIndex, out int outputIndex)
        {
            var traverseIndex = startIndex;
            if (input[startIndex] == '<')
            {
                traverseIndex = BracketScope(input, ++traverseIndex);
            }
            else if (input[startIndex] == ':')
            {
                traverseIndex++;
            }
            else if (input[startIndex] == '\'')
            {
                traverseIndex++;
                while (traverseIndex < input.Length && input[traverseIndex] != '\'') traverseIndex++;
                traverseIndex++;
            }
            else if (input[startIndex] == '\"')
            {
                traverseIndex++;
                while (traverseIndex < input.Length && input[traverseIndex] != '\"') traverseIndex++;
                traverseIndex++;
            }
            else
            {
                while (traverseIndex < input.Length && IsPrintableCharacter(input[traverseIndex])) traverseIndex++;
            }

            outputIndex = traverseIndex;

            return input.Substring(startIndex, outputIndex - startIndex);
        }

        private bool IsPrintableCharacter(char character)
        {
            return character != ' ';
        }

        private int BracketScope(string input, int start)
        {
            while (start < input.Length && input[start] != '>')
            {
                if (input[start] == '<')
                {
                    start = BracketScope(input, ++start);
                }
                else
                {
                    start++;
                }
            }
            return ++start;
        }


    }
}
