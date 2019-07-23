using System;
using System.IO;
using System.Threading.Tasks;
using HumanErrorProject.Data.Dtos;
using HumanErrorProject.Data.Models;
using HumanErrorProject.Engine.Data;
using HumanErrorProject.Engine.Options;
using HumanErrorProject.Engine.Utilities;
using Microsoft.Extensions.Options;

namespace HumanErrorProject.Engine
{
    public class Engine : IEngine
    {
        public EngineOptions Options { get; }
        protected IEngineRunner Runner { get; }
        protected IEmailService EmailService { get; }
        protected IEngineLogger Logger { get; } 

        public Engine(IOptions<EngineOptions> options, IEngineRunner runner, IEmailService emailService, IEngineLogger logger)
        {
            Runner = runner;
            EmailService = emailService;
            Logger = logger;
            Options = options.Value;
        }

        public async Task RunSubmission(StudentSubmissionDto submission)
        {
            try
            {
                await RunImplementation(submission);
            }
            catch (EngineExceptionData exceptionData)
            {
                LogEngineException(exceptionData);
            }
            catch (Exception exception)
            {
                LogException(exception, submission);
            }
        }

        public async Task RunPreAssignment(PreAssignment assignment)
        {
            try
            {
                await RunImplementation(assignment);
            }
            catch (Exception exception)
            {
                LogException(exception, assignment);
            }
        }

        public async Task RunMarkovModel(Assignment assignment, MarkovModelOptions options)
        {
            try
            {
                await RunMarkovModelImpl(assignment, options);
            }
            catch (Exception exception)
            {
                LogException(exception, assignment);
            }
        }

        private async Task RunMarkovModelImpl(Assignment assignment, MarkovModelOptions options)
        {
            using (var handler =
                new DirectoryHandler(Path.Combine(Options.RootDirectory, GetUniqueFolderName(Options.RootDirectory))))
            {
                await Runner.RunMarkovModel(assignment, options, handler);
            }
        }

        private void LogException(Exception exception, Assignment assignment)
        {
            Logger.Log($"Class: '{assignment.CourseClass.Name}'\r\n" +
                       $"Assignment: '{assignment.Name}'\r\n" +
                       $"Timestamp: {DateTime.Now}\r\n" +
                       $"Engine Message - \r\n{exception.Message}\r\n");
        }

        private void LogException(Exception exception, PreAssignment assignment)
        {
            Logger.Log($"Class: '{assignment.CourseClass.Name}'\r\n" +
                       $"Assignment: '{assignment.Name}'\r\n" +
                       $"Timestamp: {DateTime.Now}\r\n" +
                       $"Engine Message - \r\n{exception.Message}\r\n");
        }

        private async Task RunImplementation(PreAssignment assignment)
        {
            using (var handler = new DirectoryHandler(
                Path.Combine(Options.RootDirectory, GetUniqueFolderName(Options.RootDirectory))))
            {
                await Runner.RunPreAssignment(assignment, handler);
            }
        }

        private string GetUniqueFolderName(string root)
        {
            string folder;
            do
            {
                folder = Guid.NewGuid().ToString().Substring(0, 5);
            } while (Directory.Exists(Path.Combine(root, folder)));
            return folder;
        }

        private async Task RunImplementation(StudentSubmissionDto submission)
        {
            using (var data = new SubmissionData(submission,
                Path.Combine(Options.RootDirectory, GetUniqueFolderName(Options.RootDirectory))))
            {
                var email = await Runner.RunSubmission(data);
                await EmailService.Send(email);
            }
        }

        private void LogEngineException(EngineExceptionData exception)
        {
            Logger.Log($"Student: '{exception.StudentName}'\r\n" +
                       $"Class: '{exception.ClassName}'\r\n" +
                       $"Timestamp: {exception.TimeStamp}\r\n" +
                       $"Engine Message - \r\n{exception.Message}\r\n");
        }

        private void LogException(Exception exception, StudentSubmissionDto submission)
        {
            Logger.Log($"Student: '{submission.StudentName}'\r\n" +
                       $"Class: '{submission.ClassName}'\r\n" +
                       $"Timestamp: {DateTime.Now}\r\n" +
                       $"Exception Message - \r\n{exception.Message}\r\n" +
                       $"Inner Message - \r\n{exception.InnerException?.Message}\r\n");
        }

    }
}
