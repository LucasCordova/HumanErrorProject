using System.Threading.Tasks;
using HumanErrorProject.Data.Models;
using HumanErrorProject.Engine.Utilities;

namespace HumanErrorProject.Engine.Generators
{
    public interface IAssignmentGenerator
    {
        Task Generate(PreAssignment assignment, DirectoryHandler handler);
    }
}
