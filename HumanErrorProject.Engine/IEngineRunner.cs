using System.Threading.Tasks;
using HumanErrorProject.Data.Models;
using HumanErrorProject.Engine.Data;
using HumanErrorProject.Engine.Utilities;

namespace HumanErrorProject.Engine
{
    public interface IEngineRunner
    {
        Task<EmailData> Run(SubmissionData data);
        Task Run(PreAssignment assignment, DirectoryHandler directory);
    }
}
