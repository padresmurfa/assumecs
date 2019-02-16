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
            var callerId = new SourceCodeLocation("id", "memberName", "absoluteSourceFilePath", 1234);
            var ex = new Exception();
            var failure = new AssumptionFailure("message",ex, callerId);
            Assert.Equal("message", failure.Message);
            Assert.Same(ex, failure.InnerException);
            Assert.Same( callerId, failure.SourceCodeLocation);
        }

        [Fact]
        public void DoesntAcceptLocationNull()
        {
            try
            {
                new AssumptionFailure("message", null, null);
                Assert.False(true);
            }
            catch (ArgumentNullException)
            {
            }
        }
    }
}
