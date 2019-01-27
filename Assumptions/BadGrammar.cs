using System;
using System.Runtime.CompilerServices;

namespace Assumptions
{
    public class BadGrammar : Exception
    {
        public string CallerMemberName;
        public string CallerSourceFilePath;
        public int CallerSourceLineNumber;

        public BadGrammar(string message, 
            [CallerMemberName] string callerMemberName = "",
            [CallerFilePath] string callerSourceFilePath = "",
            [CallerLineNumber] int callerSourceLineNumber = 0)
            : base(message)
        {
            this.CallerMemberName = callerMemberName;
            this.CallerSourceFilePath = callerSourceFilePath;
            this.CallerSourceLineNumber = callerSourceLineNumber;
        }
    }
}
