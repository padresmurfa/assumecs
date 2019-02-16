using System;

namespace Assumptions
{
    public static class OnAssumptionFailure
    {
        public static void Create(string message, Exception innerException, SourceCodeLocation sourceCodeLocation)
        {
            throw new AssumptionFailure(message, innerException, sourceCodeLocation);
        }
        
        public static void Create(string genericMessage, string specificMessage, Exception innerException, SourceCodeLocation sourceCodeLocation)
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
            throw new AssumptionFailure(reason, innerException, sourceCodeLocation);
        }

    }
}