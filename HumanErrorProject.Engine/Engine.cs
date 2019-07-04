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

        private void LogException(Exception exception, PreAssignment assignment)
        {
            Logger.Log($"Class: '{assignment.CourseClass.Name}'\n" +
                       $"Assignment: '{assignment.Name}'\n" +
                       $"Timestamp: {DateTime.Now}\n" +
                       $"Engine Message - \n{exception.Message}");
        }

        private async Task RunImplementation(PreAssignment assignment)
        {
            using (var handler = new DirectoryHandler(
                Path.Combine(Options.RootDirectory, GetUniqueFolderName(Options.RootDirectory))))
            {
                await Runner.Run(assignment, handler);
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
                var email = await Runner.Run(data);
                await EmailService.Send(email);
            }
        }

        private void LogEngineException(EngineExceptionData exception)
        {
            Logger.Log($"Student: '{exception.StudentName}'\n" +
                       $"Class: '{exception.ClassName}'\n" +
                       $"Timestamp: {exception.TimeStamp}\n" +
                       $"Engine Message - \n{exception.Message}");
        }

        private void LogException(Exception exception, StudentSubmissionDto submission)
        {
            Logger.Log($"Student: '{submission.StudentName}'\n" +
                       $"Class: '{submission.ClassName}'\n" +
                       $"Timestamp: {DateTime.Now}\n" +
                       $"Exception Message - \n{exception.Message}\n" +
                       $"Inner Message - \n{exception.InnerException?.Message}");
        }

    }
}
