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
                Assert.False(true);
            }
            catch (AssumptionFailure ex)
            {
                Assert.Equal("Expected this code to be unreachable.  Explanation: UnreachableCode", ex.Message);
                Assert.Null(ex.InnerException);
                Assert.Equal(nameof(UnreachableIsThrown), ex.CallerMemberName);
                Assert.True(ex.CallerSourceFilePath.EndsWith(System.IO.Path.Combine("Assumptions", "UnitTests","AssumeTests.cs")));
                Assert.Equal(14, ex.CallerSourceLineNumber);
            }
        }
        
        [Fact]
        public void ThatWorks()
        {
            // there is no public interface for querying an assumption in this
            // state, so nothing more we can test really, unless we change the
            // interface.  Not neccessary however, since this will be tested
            // elsewhere
            var that = Assume.That("actual", "name");
            Assert.NotNull(that);
        }
    }
}
