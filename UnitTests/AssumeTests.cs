using System;
using Xunit;
using Assumptions;

namespace UnitTests
{
    public class AssumeTests
    {
        [Fact]
        public void UnreachableIsThrown()
        {
            try
            {
                Assume.Unreachable("UnreachableCode"); // line number: 14

                throw new Exception("Assumption was not thrown");
            }
            catch (AssumptionFailure ex)
            {
                Assert.Equal("Expected this code to be unreachable.  Explanation: UnreachableCode", ex.Message);
                Assert.Null(ex.InnerException);
                Assert.Equal(nameof(UnreachableIsThrown), ex.SourceCodeLocation.MemberName);
                Assert.EndsWith(System.IO.Path.Combine("UnitTests","AssumeTests.cs"), ex.SourceCodeLocation.AbsoluteSourceFilePath);
                Assert.Equal(14, ex.SourceCodeLocation.SourceLineNumber);
            }
        }
        
        [Fact]
        public void ThatWorks()
        {
            // there is no public interface for querying an assumption in this
            // state, so nothing more we can test really, unless we change the
            // interface.  Not neccessary however, since this will be tested
            // elsewhere
            var that = Assume.That("actual");
            Assert.NotNull(that);
        }
    }
}
