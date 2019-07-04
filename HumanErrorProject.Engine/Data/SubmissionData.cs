using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using HumanErrorProject.Data.Dtos;
using HumanErrorProject.Data.Models;
using HumanErrorProject.Engine.Utilities;
using Microsoft.EntityFrameworkCore.Internal;

namespace HumanErrorProject.Engine.Data
{
    public class SubmissionData : IDisposable
    {
        public string Root { get; }
        public string SnapshotFolder { get; }
        public string StudentName { get; }
        public string ClassName { get; }
        public Student Student { get; set;  }
        public CourseClass Course { get; set; }

        public SubmissionData(StudentSubmissionDto submission, string root)
        {
            StudentName = submission.StudentName;
            ClassName = submission.ClassName;

            Root = root;
            Directory.CreateDirectory(Root);

            SnapshotFolder = Path.Combine(Root, "SnapshotFiles");
            EngineFileUtilities.ExtractZip(Root, "SnapshotFiles", submission.SnapshotFolder);
        }

        public IEnumerable<string> SnapshotFolderNames() 
            => Directory.GetDirectories(SnapshotFolder).Select(f => new DirectoryInfo(f).Name);

        public bool HasSourceFile(string snapshot, string filename) 
            => Directory.GetFiles(Path.Combine(SnapshotFolder, snapshot)).Any(f => Path.GetFileName(f).Equals(filename));

        public IEnumerable<string> SnapshotSourceFiles(string snapshot) => Directory.GetFiles(Path.Combine(SnapshotFolder, snapshot));

        public string SnapshotSourceFileFullPath(string snapshot, string filename)
            => Path.Combine(SnapshotFolder, snapshot, filename);

        public string SnapshotFullPath(string snapshot)
            => Path.Combine(SnapshotFolder, snapshot);

        public void Dispose()
        {
            if (Directory.Exists(Root))
            {
                Directory.Delete(Root, true);
            }
        }
    }
}
