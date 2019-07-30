using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HumanErrorProject.Data.DataAccess;
using HumanErrorProject.Data.Models;
using HumanErrorProject.Engine.Analysis.AbstractSyntaxTree;
using HumanErrorProject.Engine.Data;
using HumanErrorProject.Engine.Utilities;

namespace HumanErrorProject.Engine.Generators
{
    public class SnapshotGenerator : ISnapshotGenerator
    {
        protected ISnapshotDateConverter Converter;
        protected IRepository<SnapshotSubmission, int> SnapshotSubmissions;
        protected IRepository<Snapshot, int> Snapshots;
        protected ISnapshotReportGenerator ReportGenerator;
        protected IRepository<Student, string> Students;
        protected IAbstractSyntaxTreeClassExtractor ClassExtractor;
        protected IAbstractSyntaxTreeGenerator AbstractSyntaxTreeGenerator;

        public SnapshotGenerator(ISnapshotDateConverter converter, IRepository<SnapshotSubmission, int> snapshotSubmissions, IRepository<Snapshot, int> snapshots, ISnapshotReportGenerator reportGenerator, IRepository<Student, string> students, IAbstractSyntaxTreeClassExtractor classExtractor, IAbstractSyntaxTreeGenerator abstractSyntaxTreeGenerator)
        {
            Converter = converter;
            SnapshotSubmissions = snapshotSubmissions;
            Snapshots = snapshots;
            ReportGenerator = reportGenerator;
            Students = students;
            ClassExtractor = classExtractor;
            AbstractSyntaxTreeGenerator = abstractSyntaxTreeGenerator;
        }

        public async Task<IList<Snapshot>> Generate(SubmissionData data, Assignment assignment)
        {
            var newSnapshots = new List<Snapshot>();
            var generateObj = new SnapshotGeneratorObj(data, assignment, this);

            var solutionNode = GetSolutionAbstractSyntaxTreeNode(data, assignment);

            foreach (var snapshotName in generateObj.SnapshotNames())
            {
                if (generateObj.IsNewSnapshot(snapshotName))
                {
                    var snapshotSubmission = await generateObj.AddSubmissionToStudentSubmission(snapshotName);
                    var snapshot = new Snapshot()
                    {
                        StudentId = generateObj.Data.Student.Id,
                        AssignmentId = generateObj.Assignment.Id,
                        SnapshotSubmission = snapshotSubmission,
                        Report = await ReportGenerator.Generate(generateObj.Data, snapshotName, assignment, solutionNode),
                    };
                    await Snapshots.Add(snapshot);
                    newSnapshots.Add(snapshot);
                    generateObj.LastSnapshot = snapshot;
                }
            }
            return newSnapshots;
        }

        public AbstractSyntaxTreeNode GetSolutionAbstractSyntaxTreeNode(SubmissionData data, Assignment assignment)
        {
            using (var directory = new DirectoryHandler(Path.Combine(data.Root, nameof(SnapshotGenerator))))
            {
                var snapshotPath = EngineFileUtilities.ExtractZip(directory.Directory,
                    "Solution", assignment.Solution.Files);
                var file = Path.Combine(snapshotPath, assignment.Filename);
                var solution = AbstractSyntaxTreeGenerator.CreateFromFile(directory, file);
                return ClassExtractor.Extract(solution, assignment.Solution.Name);
            }
        }

        public class SnapshotGeneratorObj
        {
            public SubmissionData Data { get; private set; }
            public Assignment Assignment { get; private set; }
            public Snapshot LastSnapshot { get; set; }
            public SnapshotGenerator Parent { get; }

            public SnapshotGeneratorObj(SubmissionData data, Assignment assignment, SnapshotGenerator parent)
            {
                Data = data;
                Assignment = assignment;
                Parent = parent;
                LastSnapshot = GetLastSnapshotOrDefault(data, assignment);
            }

            private static Snapshot GetLastSnapshotOrDefault(SubmissionData data, Assignment assignment)
            {
                return data.Student.Snapshots
                    .Where(s => s.AssignmentId.Equals(assignment.Id))
                    .OrderByDescending(s => s.SnapshotSubmission.CreatedDateTime).FirstOrDefault();
            }

            public IList<string> SnapshotNames()
            {
                var snapshotsNames = Data.SnapshotFolderNames()
                    .Where(s => Data.HasSourceFile(s, Assignment.Filename) && NewValidSnapshot(s)).ToList();
                return snapshotsNames;
            }

            public bool NewValidSnapshot(string snapshot)
            {
                if (!Parent.Converter.CanConvert(snapshot)) return false;
                if (LastSnapshot == null) return true;
                var date = Parent.Converter.Convert(snapshot);
                return date > LastSnapshot.SnapshotSubmission.CreatedDateTime;
            }

            public bool AreSnapshotsTheSame(string pathToNewSnapshot, string filename)
            {
                using (var directory = new DirectoryHandler(Path.Combine(Data.Root, nameof(SnapshotGenerator))))
                {
                    var snapshotPath = EngineFileUtilities.ExtractZip(directory.Directory,
                        "Snapshot", LastSnapshot.SnapshotSubmission.Files);
                    var expectedFilePath = Path.Combine(snapshotPath, filename);
                    return EngineFileUtilities.SameFile(expectedFilePath, pathToNewSnapshot);
                }
            }

            public async Task<SnapshotSubmission> AddSubmissionToStudentSubmission(string snapshotName)
            {
                var date = Parent.Converter.Convert(snapshotName);
                foreach (var submission in Data.Student.Submissions)
                {
                    if (submission.CreatedDateTime.Equals(date))
                        return submission;
                }
                var snapshotSubmission = new SnapshotSubmission()
                {
                    CreatedDateTime = date,
                    Files = EngineFileUtilities.ZipFiles(Data.SnapshotFullPath(snapshotName)),
                    StudentId = Data.Student.Id,
                };
                await Parent.SnapshotSubmissions.Add(snapshotSubmission);
                return snapshotSubmission;
            }

            public bool IsNewSnapshot(string snapshotName)
            {
                if (LastSnapshot == null) return true;
                return !AreSnapshotsTheSame(
                    Data.SnapshotSourceFileFullPath(snapshotName, Assignment.Filename),
                    Assignment.Filename);
            }
        }
    }
}
