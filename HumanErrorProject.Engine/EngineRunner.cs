using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HumanErrorProject.Data.DataAccess;
using HumanErrorProject.Data.DataAccess.Repositories;
using HumanErrorProject.Data.Models;
using HumanErrorProject.Engine.Data;
using HumanErrorProject.Engine.Generators;
using HumanErrorProject.Engine.Options;
using HumanErrorProject.Engine.Utilities;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Options;

namespace HumanErrorProject.Engine
{
    public class EngineRunner : IEngineRunner
    {
        public EngineRunnerOptions Options { get; }
        protected IRepository<Student, int> StudentRepository { get; }
        protected IRepository<Survey, string> SurveyRepository { get; }
        protected ISnapshotGenerator SnapshotGenerator { get; }
        protected IAssignmentGenerator AssignmentGenerator { get; }

        public EngineRunner(IOptions<EngineRunnerOptions> options, IRepository<Student, int> studentRepository, IRepository<Survey, string> surveyRepository, ISnapshotGenerator snapshotGenerator,
            IAssignmentGenerator assignmentGenerator)
        {
            StudentRepository = studentRepository;
            SurveyRepository = surveyRepository;
            SnapshotGenerator = snapshotGenerator;
            AssignmentGenerator = assignmentGenerator;
            Options = options.Value;
        }

        public async Task<EmailData> Run(SubmissionData data)
        {
            data.Student = await GetStudent(data);
            data.Course = await GetCourseClass(data.Student, data);
            return await GetEmailData(data);
        }

        public async Task Run(PreAssignment assignment, DirectoryHandler directory)
        {
            await AssignmentGenerator.Generate(assignment, directory);
        }

        public async Task<Student> GetStudent(SubmissionData data)
        {
            var student = await StudentRepository.SingleOrDefault(s => s.Name.Equals(data.StudentName));

            if (student == null)
                throw new EngineExceptionData($"Student '{data.StudentName}' does not exists in the database", data);

            return student;
        }

        public Task<CourseClass> GetCourseClass(Student student, SubmissionData data)
        {
            var studentCourses = student.StudentCourseClasses
                .SingleOrDefault(c => c.Class.Name.Equals(data.ClassName));

            if (studentCourses == null)
                throw new EngineExceptionData($"Class '{data.ClassName}' does not exists in the database", data);

            return Task.FromResult(studentCourses.Class);
        }

        public async Task<EmailData> GetEmailData(SubmissionData data)
        {
            switch (data.Course.Assignments.Count)
            {
                case 0:
                    return GetNoAssignmentsEmail(data);
                default:
                    return await GenerateAssignments(data);
            }
        }

        public async Task<EmailData> GenerateAssignments(SubmissionData data)
        {
            var assignmentSnapshots = new List<List<Snapshot>>();
            foreach (var assignment in data.Course.Assignments)
            {
                var snapshots = await SnapshotGenerator.Generate(data, assignment);

                assignmentSnapshots.Add(snapshots.ToList());
            }

            if (HasSnapshotsToReport(assignmentSnapshots))
            {
                var survey = await GenerateSurvey(data, assignmentSnapshots);
                return GetSurveyReport(survey);
            }

            return NoSnapshotsToReport(data);
        }

        public EmailData GetSurveyReport(Survey survey)
        {
            return new EmailData(survey.Student)
            {
                Subject = "Survey Report",
                Content = $"Follow link to Survey: {Options.SurveyUrl}{survey.Id}",
            };
        }

        public async Task<Survey> GenerateSurvey(SubmissionData data, List<List<Snapshot>> assignmentSnapshots)
        {
            var snapshots = assignmentSnapshots.SelectMany(s => s);
            var survey = new Survey()
            {
                Snapshots = snapshots.ToList(),
                Student = data.Student,
            };
            await SurveyRepository.Add(survey);
            return survey;
        }

        public bool HasSnapshotsToReport(List<List<Snapshot>> assignmentSnapshots)
        {
            return assignmentSnapshots.Any(s => s.Any());
        }

        public EmailData NoSnapshotsToReport(SubmissionData data)
        {
            return new EmailData(data.Student)
            {
                Subject = "No Survey Report",
                Content = $"No snapshots to report. " +
                          $"Either the files haven't changed or the snapshots doesn't contain the required assignments.\n" +
                          $"Below is a list of assignment's files that we're looking for:\n" +
                          $"{data.Course.Assignments.Select(s => $"- {s.Name}, {s.Filename}").Join("\n")}\n"
            };
        }

        public EmailData GetNoAssignmentsEmail(SubmissionData data)
        {
            return new EmailData(data.Student)
            {
                Subject = $"{data.Course.Name} has no assignments",
                Content = $"On {DateTime.Today.ToShortDateString()},\n" +
                          $"you submitted a snapshot package, however" +
                          $"the class \'{data.Course.Name}\' has no assignments currently.\n"
            };
        }
    }
}
