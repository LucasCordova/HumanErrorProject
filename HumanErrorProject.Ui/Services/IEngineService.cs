using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HumanErrorProject.Data.Dtos;
using HumanErrorProject.Data.Models;

namespace HumanErrorProject.Ui.Services
{
    public interface IEngineService
    {
        void RunPreAssignment(int id);
        void RunSubmission(StudentSubmissionDto submission);
    }
}
