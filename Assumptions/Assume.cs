using System;
using System.Runtime.CompilerServices;

namespace Assumptions
{
    public static class Assume
    {
        public static Assumption That(
            object actual,
            SourceCodeLocation sourceCodeLocation)
        {
            return new Assumption(actual, sourceCodeLocation);
        }

        public static Assumption That(
            Action actual,
            SourceCodeLocation sourceCodeLocation)
        {
            return new Assumption(actual, sourceCodeLocation);
        }
        
        public static Assumption That(
            object actual,
            string callerId = null,
            [CallerMemberName] string callerMemberName = "",
            [CallerFilePath] string callerSourceFilePath = "",
            [CallerLineNumber] int callerSourceLineNumber = 0)
        {
            return Assume.That(actual, new SourceCodeLocation(callerId, callerMemberName, callerSourceFilePath, callerSourceLineNumber));
        }
        
        public static Assumption That(
            Action actual,
            string callerId = null,
            [CallerMemberName] string callerMemberName = "",
            [CallerFilePath] string callerSourceFilePath = "",
            [CallerLineNumber] int callerSourceLineNumber = 0)
        {
            return Assume.That(actual, new SourceCodeLocation(callerId, callerMemberName, callerSourceFilePath, callerSourceLineNumber));
        }
        
        public static void Unreachable(
            string message = null,
            Exception innerException = null,
            string callerId = null,
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
            
            OnAssumptionFailure.Create("Expected this code to be unreachable" + message, innerException, new SourceCodeLocation(callerId, callerMemberName, callerSourceFilePath, callerSourceLineNumber));
        }
    }
}
