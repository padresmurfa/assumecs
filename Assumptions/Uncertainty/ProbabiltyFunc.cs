using System;

namespace Assumptions.Uncertainty
{
    public abstract class ProbabiltyFunc
    {
        public abstract bool Check(bool success, Func<string> failureReasonFactory, SourceCodeLocation sourceCodeLocation);
    }
}