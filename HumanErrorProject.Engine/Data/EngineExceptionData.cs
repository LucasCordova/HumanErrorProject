using System;

namespace HumanErrorProject.Engine.Data
{
    public class EngineExceptionData : Exception
    {
        public EngineExceptionData(string message, SubmissionData data) : base(message)
        {
            StudentName = data.StudentName;
            ClassName = data.ClassName;
        }
        public DateTime TimeStamp { get; } = DateTime.Now;
        public string StudentName { get; }
        public string ClassName { get; }
    }
}
