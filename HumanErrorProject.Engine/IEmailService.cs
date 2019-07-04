using System.Threading.Tasks;
using HumanErrorProject.Engine.Data;

namespace HumanErrorProject.Engine
{
    public interface IEmailService
    {
        Task Send(EmailData data);
    }
}
