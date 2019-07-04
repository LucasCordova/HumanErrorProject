using System;

namespace HumanErrorProject.Engine.Utilities
{
    public interface ISnapshotDateConverter
    {
        bool CanConvert(string name);
        DateTime Convert(string name);
    }
}
