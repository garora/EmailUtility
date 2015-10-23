namespace Utility.Core
{
    public class ValidationResults : Errors, IValidationResults, IErrors, IMessages
    {
        public static readonly ValidationResults Empty = new ValidationResults();

        public bool IsValid
        {
            get { return Count == 0; }
        }
    }
}