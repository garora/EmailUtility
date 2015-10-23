namespace Utility.Logging
{
    public class TraceSwitch : System.Diagnostics.TraceSwitch
    {
        public TraceSwitch(string name, string description)
            : base(name, description)
        {
        }
    }
}