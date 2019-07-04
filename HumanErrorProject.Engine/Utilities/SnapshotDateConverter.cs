using System;
using System.Text.RegularExpressions;

namespace HumanErrorProject.Engine.Utilities
{
    public class SnapshotDateConverter : ISnapshotDateConverter
    {
        private readonly Regex _regex = new Regex(@"(Snapshot)(\d\d?)(-)(\d\d?)(-)(\d\d\d\d)(_)(\d\d?)(.)(\d\d?)(.)(\d\d?)(.)(\d\d?)");

        public bool CanConvert(string name)
        {
            return _regex.IsMatch(name);
        }

        public DateTime Convert(string name)
        {
            var match = _regex.Match(name);
            return new DateTime(
                int.Parse(match.Groups[6].Value),
                int.Parse(match.Groups[2].Value),
                int.Parse(match.Groups[4].Value),
                int.Parse(match.Groups[8].Value),
                int.Parse(match.Groups[10].Value),
                int.Parse(match.Groups[12].Value)
                );
        }
    }
}
