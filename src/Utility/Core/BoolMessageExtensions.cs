namespace Utility.Core
{
    public static class BoolMessageExtensions
    {
        public static int AsExitCode(this BoolMessage result)
        {
            return result.Success ? 0 : 1;
        }
    }
}