using System.Threading.Tasks;
using HumanErrorProject.Data.Models;
using HumanErrorProject.Engine.Data;
using HumanErrorProject.Engine.Options;
using HumanErrorProject.Engine.Utilities;

namespace HumanErrorProject.Engine
{
    public interface IEngineRunner
    {
        Task<EmailData> RunSubmission(SubmissionData data);
        Task RunPreAssignment(PreAssignment assignment, DirectoryHandler directory);
        Task RunMarkovModel(Assignment assignment, MarkovModelOptions options, DirectoryHandler directory);
    }
}
