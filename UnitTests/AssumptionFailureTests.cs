using System;
using Xunit;
using Assumptions;

namespace UnitTests
{
    public class AssumptionFailureTests
    {
        [Fact]
        public void ConstructorWorksAsExpected()
        {
            var ex = new Exception();
            var failure = new AssumptionFailure("message",ex,"member","file",1);
            Assert.Equal("message", failure.Message);
            Assert.Same(ex, failure.InnerException);
            Assert.Equal("member", failure.CallerMemberName);
            Assert.Equal("file", failure.CallerSourceFilePath);
            Assert.Equal(1, failure.CallerSourceLineNumber);
        }
    }
}
