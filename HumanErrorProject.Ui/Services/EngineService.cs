using System.Threading;
using System.Threading.Tasks;
using Hangfire;
using HumanErrorProject.Data.DataAccess;
using HumanErrorProject.Data.DataAccess.Repositories;
using HumanErrorProject.Data.Dtos;
using HumanErrorProject.Data.Models;
using HumanErrorProject.Engine;
using HumanErrorProject.Engine.Options;

namespace HumanErrorProject.Ui.Services
{
    public class EngineService : IEngineService
    {
        public IEngine Engine { get; }
        public IRepository<PreAssignment, int> PreAssignmentRepository { get; }
        public IRepository<Assignment, int> AssignmentRepository { get; }

        public static Mutex Mutex = new Mutex();
        public IHangFireJobService HangFireJobService;

        public EngineService(IEngine engine, IRepository<PreAssignment, int> preAssignmentRepository, IHangFireJobService hangFireJobService, IRepository<Assignment, int> assignmentRepository)
        {
            Engine = engine;
            PreAssignmentRepository = preAssignmentRepository;
            HangFireJobService = hangFireJobService;
            AssignmentRepository = assignmentRepository;
        }


        [AutomaticRetry(Attempts = 0)]
        public async Task RunPreAssignmentImpl(int id)
        {
            var assignment = await PreAssignmentRepository.Get(id);
            await Engine.RunPreAssignment(assignment);
        }

        [AutomaticRetry(Attempts = 0)]
        public async Task RunSubmissionImpl(StudentSubmissionDto submission)
        {
            await Engine.RunSubmission(submission);
            Mutex.WaitOne();
            HangFireJobService.Remove(submission.StudentName);
            Mutex.ReleaseMutex();
        }

        [AutomaticRetry(Attempts = 0)]
        public async Task RunMarkovModelImpl(int id, MarkovModelOptions options)
        {
            var assignment = await AssignmentRepository.Get(id);
            await Engine.RunMarkovModel(assignment, options);
        }

        public void RunMarkovModel(int id, MarkovModelOptions options)
        {
            BackgroundJob.Enqueue(() => RunMarkovModelImpl(id, options));
        }


        public void RunPreAssignment(int id)
        {
            BackgroundJob.Enqueue(() => RunPreAssignmentImpl(id));
        }

        public void RunSubmission(StudentSubmissionDto submission)
        {
            Mutex.WaitOne();

            var parentId = HangFireJobService.GetParentOrDefault(submission.StudentName);

            if (parentId == null)
            {
                var newId = BackgroundJob.Enqueue(() => RunSubmissionImpl(submission));
                HangFireJobService.AddJob(submission.StudentName, newId);
            }
            else
            {
                var newId = BackgroundJob.ContinueJobWith(parentId, () => RunSubmissionImpl(submission));
                HangFireJobService.AddJob(submission.StudentName, newId);
            }

            Mutex.ReleaseMutex();
        }
    }
}
