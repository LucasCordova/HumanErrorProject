namespace HumanErrorProject.Engine.Data
{
    public class EngineProcessData
    {
        public string Command { get; set; }
        public string Arguments { get; set; } = "";
        public string WorkingDirectory { get; set; }
        public int WaitForExit { get; set; } = 180000;
    }
}
