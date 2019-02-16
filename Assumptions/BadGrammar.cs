using System;
using System.Runtime.CompilerServices;

namespace Assumptions
{
    public class BadGrammar : Exception
    {
        public SourceCodeLocation SourceCodeLocation;

        public BadGrammar(string message, 
            string callerId = null,
            [CallerMemberName] string callerMemberName = "",
            [CallerFilePath] string callerSourceFilePath = "",
            [CallerLineNumber] int callerSourceLineNumber = 0)
            : base(message)
        {
            this.SourceCodeLocation = new SourceCodeLocation(callerId, callerMemberName, callerSourceFilePath, callerSourceLineNumber);
        }
    }
}
