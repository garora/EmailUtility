using System;

namespace Utility.Core
{
    public class ValidationRuleDef
    {
        public string Name;
        public Func<ValidationEvent, bool> Rule;
    }
}