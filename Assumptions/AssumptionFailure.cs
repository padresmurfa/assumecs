using System;

namespace BrokenArrow
{
    public class AssumptionFailure : Exception
    {
        public AssumptionFailure(string message)
            : base(message)
        {
        }
    }
}
