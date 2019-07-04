using System.Threading.Tasks;
using HumanErrorProject.Data.Dtos;
using HumanErrorProject.Data.Models;

namespace HumanErrorProject.Engine
{
    public interface IEngine
    {
        Task RunSubmission(StudentSubmissionDto submission);
        Task RunPreAssignment(PreAssignment assignment);
    }
}
