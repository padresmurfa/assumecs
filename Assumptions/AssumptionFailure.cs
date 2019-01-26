using System;

namespace Assumptions
{
    public class AssumptionFailure : Exception
    {
        public string CallerMemberName;
        public string CallerSourceFilePath;
        public int CallerSourceLineNumber;

        public AssumptionFailure(string message, Exception innerException, string callerMemberName, string callerSourceFilePath, int callerSourceLineNumber)
            : base(message, innerException)
        {
            this.CallerMemberName = callerMemberName;
            this.CallerSourceFilePath = callerSourceFilePath;
            this.CallerSourceLineNumber = callerSourceLineNumber;
        }
    }
}
