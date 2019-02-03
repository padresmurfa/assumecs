using System;

namespace Assumptions
{
    public static class OnAssumptionFailure
    {
        public static void Create(string message, Exception innerException, string callerMemberName,
            string callerSourceFilePath, int callerSourceLineNumber)
        {
            throw new AssumptionFailure(message, innerException, callerMemberName, callerSourceFilePath, callerSourceLineNumber);
        }
        
        public static void Create(string genericMessage, string specificMessage, Exception innerException, string callerMemberName,
            string callerSourceFilePath, int callerSourceLineNumber)
        {
            string reason;
            if (string.IsNullOrEmpty(specificMessage))
            {
                reason = genericMessage;
            }
            else if (string.IsNullOrEmpty(genericMessage))
            {
                reason = specificMessage;
            }
            else
            {
                reason = specificMessage + ". " + genericMessage;
            }
            throw new AssumptionFailure(reason, innerException, callerMemberName, callerSourceFilePath, callerSourceLineNumber);
        }

    }
}