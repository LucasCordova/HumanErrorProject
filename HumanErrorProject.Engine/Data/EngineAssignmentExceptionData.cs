using System;
using HumanErrorProject.Data.Models;

namespace HumanErrorProject.Engine.Data
{
    public class EngineAssignmentExceptionData : Exception
    {
        public PreAssignmentReport Report { get; set; }
    }
}
