namespace Utility.Core
{
    public interface IValidationResults : IErrors, IMessages
    {
        bool IsValid { get; }
    }
}