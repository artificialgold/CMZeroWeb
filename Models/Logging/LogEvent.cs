namespace CMZero.Web.Models.Logging
{
    public class LogEvent
    {
        public LogEvent(string message)
        {
            Message = message;
        }

        //ADD PROPERTIES WHEN KNOW WHAT TO LOG 
        public string Message { get; private set; }
    }
}