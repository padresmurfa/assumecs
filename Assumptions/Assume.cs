using System;
using System.Runtime.CompilerServices;

namespace Assumptions
{
    public static class Assume
    {
        public static Assumption That(
            object actual,
            string actualName = null,
            [CallerMemberName] string callerMemberName = "",
            [CallerFilePath] string callerSourceFilePath = "",
            [CallerLineNumber] int callerSourceLineNumber = 0)
        {
            return new Assumption(actual, actualName, callerMemberName, callerSourceFilePath, callerSourceLineNumber);
        }
        
        public static void Unreachable(
            string message = null,
            Exception innerException = null,
            [CallerMemberName] string callerMemberName = "",
            [CallerFilePath] string callerSourceFilePath = "",
            [CallerLineNumber] int callerSourceLineNumber = 0)
        {
            if (message == null)
            {
                message = "";
            }
            else
            {
                message = ".  Explanation: " + message;
            }
            throw new AssumptionFailure("Expected this code to be unreachable" + message, innerException, callerMemberName, callerSourceFilePath, callerSourceLineNumber);
        }
    }
}
