using System;

namespace Assumptions
{
    public class AssumptionFailure : Exception
    {
        public AssumptionFailure(string message)
            : base(message)
        {
        }
    }
}
