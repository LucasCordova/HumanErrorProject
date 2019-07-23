using System.IO;
using HumanErrorProject.Data.Models;
using HumanErrorProject.Engine.Analysis.AbstractSyntaxTree;
using HumanErrorProject.Engine.Data;
using HumanErrorProject.Engine.Options;
using HumanErrorProject.Engine.Utilities;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

namespace HumanErrorProject.Engine.Generators
{
    public class ClangAbstractSyntaxTreeGenerator : IAbstractSyntaxTreeGenerator
    {
        public ClangOptions Options { get; }
        protected IAbstractSyntaxTreeExtractor Extractor;

        public ClangAbstractSyntaxTreeGenerator(IOptions<ClangOptions> options,
            IAbstractSyntaxTreeExtractor extractor)
        {
            Extractor = extractor;
            Options = options.Value;
        }

        public AbstractSyntaxTreeNode CreateFromFile(SubmissionData data, string path)
        {
            using (var handler =
                new DirectoryHandler(GetCompilationDirectory(data)))
            {
                var process = GetEngineProcess(GetEngineProcessData(handler.Directory, path));
                var exitCode = process.Run();
                if (exitCode == 0)
                {
                    process.Stop();
                    using (var reader = new StreamReader(GetOutputFile(handler.Directory)))
                    {
                        return Extractor.Extract(reader);
                    }
                }
                EngineReportExceptionData exception;

                using (var reader = process.StandardError)
                {
                    exception = new EngineReportExceptionData(reader.ReadToEnd())
                    {
                        Type = "Compilation",
                    };
                }
                process.Stop();
                throw exception;
            }
        }

        public AbstractSyntaxTreeNode CreateFromFile(DirectoryHandler handler, string path)
        {
            ValidatePath(path);
            using (var comiplerHandler = new DirectoryHandler(GetCompilationDirectory(handler)))
            {
                var process = GetEngineProcess(GetEngineProcessData(comiplerHandler.Directory, path));
                var exitCode = process.Run();
                if (exitCode == 0)
                {
                    process.Stop();
                    using (var reader = new StreamReader(GetOutputFile(comiplerHandler.Directory)))
                    {
                        return Extractor.Extract(reader);
                    }
                }
                EngineAssignmentExceptionData exception;
                using (var reader = process.StandardError)
                {
                    exception = new EngineAssignmentExceptionData()
                    {
                        Report = new PreAssignmentCompileFailureReport()
                        {
                            Report = reader.ReadToEnd()
                        }
                    };
                }
                process.Stop();
                throw exception;
            }
        }

        public AbstractSyntaxTreeNode CreateOrDefaultFromFile(DirectoryHandler handler, string path)
        {
            using (var comiplerHandler = new DirectoryHandler(GetCompilationDirectory(handler)))
            {
                var process = GetEngineProcess(GetEngineProcessData(comiplerHandler.Directory, path));
                var exitCode = process.Run();
                if (exitCode == 0)
                {
                    process.Stop();
                    using (var reader = new StreamReader(GetOutputFile(comiplerHandler.Directory)))
                    {
                        return Extractor.Extract(reader);
                    }
                }
                process.Stop();
                return null;
            }
        }

        private void ValidatePath(string path)
        {
            if (!File.Exists(path))
            {
                throw new EngineAssignmentExceptionData()
                {
                    Report = new PreAssignmentNoFileFailureReport()
                };
            }
        }

        public string GetCompilationDirectory(SubmissionData data)
            => Path.Combine(data.Root, nameof(ClangAbstractSyntaxTreeGenerator));
        public string GetCompilationDirectory(DirectoryHandler handler)
            => Path.Combine(handler.Directory, nameof(ClangAbstractSyntaxTreeGenerator));


        public EngineProcessData GetEngineProcessData(string workingDirectroy, string sourceFile)
        {
            return new EngineProcessData()
            {
                Command = Options.Command,
                Arguments = $"{Options.Arguments} \'{sourceFile}\' > \'{GetOutputFile(workingDirectroy)}\'",
                WorkingDirectory = workingDirectroy
            };
        }

        public string GetOutputFile(string directory) => Path.Combine(directory, Options.OutputFile);

        public EngineProcess GetEngineProcess(EngineProcessData data)
        {
            return new EngineProcess(data);
        }

    }
}
