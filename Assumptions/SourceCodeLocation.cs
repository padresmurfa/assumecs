using System.Runtime.CompilerServices;

namespace Assumptions
{
    public class SourceCodeLocation
    {
        public readonly string Id;
        public readonly string MemberName;
        public readonly string AbsoluteSourceFilePath;
        public readonly int SourceLineNumber;
        
        public SourceCodeLocation(
                string id,
                bool dontCallMe = false,
                [CallerMemberName] string callerMemberName = "",
                [CallerFilePath] string callerSourceFilePath = "",
                [CallerLineNumber] int callerSourceLineNumber = 0)
            : this(id, callerMemberName, callerSourceFilePath, callerSourceLineNumber)
        {
        }

        public SourceCodeLocation(string id, string memberName, string absoluteSourceFilePath, int sourceLineNumber)
        {
            this.Id = id;
            this.MemberName = memberName;
            this.AbsoluteSourceFilePath = absoluteSourceFilePath;
            this.SourceLineNumber = sourceLineNumber;
        }

        public SourceCodeLocation Here(
            string id = null,
            [CallerMemberName] string callerMemberName = "",
            [CallerFilePath] string callerSourceFilePath = "",
            [CallerLineNumber] int callerSourceLineNumber = 0)
        {
            return new SourceCodeLocation(id ?? Id, this.MemberName, this.AbsoluteSourceFilePath, this.SourceLineNumber);
        }

        private long? _key;
        public long Key
        {
            get
            {
                if (!_key.HasValue)
                {
                    var high = (long) AbsoluteSourceFilePath.GetHashCode() << 32;
                    var low = (long) MemberName.GetHashCode();
                    var xor = SourceLineNumber;

                    var result = (high + low) ^ xor;

                    if (!string.IsNullOrEmpty(Id))
                    {
                        result ^= Id.GetHashCode();
                    }

                    _key = result;
                }

                return _key.Value;
            }
        }
    }
}