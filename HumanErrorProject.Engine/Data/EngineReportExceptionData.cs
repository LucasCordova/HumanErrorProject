using System;

namespace HumanErrorProject.Engine.Data
{
    public class EngineReportExceptionData : Exception
    {
        public EngineReportExceptionData(string message) : base(message)
        {

        }
        public string Type { get; set; }
    }
}
