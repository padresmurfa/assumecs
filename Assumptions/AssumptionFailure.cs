using System;

namespace Assumptions
{
    public class AssumptionFailure : Exception
    {
        public readonly SourceCodeLocation SourceCodeLocation;

        public AssumptionFailure(string message, Exception innerException, SourceCodeLocation sourceCodeLocation)
            : base(message, innerException)
        {
            this.SourceCodeLocation = sourceCodeLocation ?? throw new ArgumentNullException(nameof(sourceCodeLocation));
        }
    }
}
