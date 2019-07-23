using System.Threading.Tasks;
using HumanErrorProject.Data.Dtos;
using HumanErrorProject.Data.Models;
using HumanErrorProject.Engine.Options;

namespace HumanErrorProject.Engine
{
    public interface IEngine
    {
        Task RunSubmission(StudentSubmissionDto submission);
        Task RunPreAssignment(PreAssignment assignment);
        Task RunMarkovModel(Assignment assignment, MarkovModelOptions options);
    }
}
