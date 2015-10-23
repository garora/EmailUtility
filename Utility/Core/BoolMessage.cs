namespace Utility.Core
{
    public class BoolMessage
    {
        public static readonly BoolMessage True = new BoolMessage(true, string.Empty);
        public static readonly BoolMessage False = new BoolMessage(false, string.Empty);
        public readonly string Message;
        public readonly bool Success;

        public BoolMessage(bool success, string message)
        {
            Success = success;
            Message = message;
        }
    }
}