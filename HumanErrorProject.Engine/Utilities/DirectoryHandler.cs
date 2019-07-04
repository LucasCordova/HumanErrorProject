using System;

namespace HumanErrorProject.Engine.Utilities
{
    public class DirectoryHandler : IDisposable
    {
        public string Directory { get; }

        public DirectoryHandler(string directory)
        {
            Directory = directory;
            System.IO.Directory.CreateDirectory(Directory);
        }

        public void Dispose()
        {
            System.IO.Directory.Delete(Directory, true);
        }
    }
}
