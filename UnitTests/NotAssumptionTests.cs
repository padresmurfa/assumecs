using System;
using Xunit;
using Assumptions;

namespace UnitTests
{
    public class NotAssumptionTests
    {
        [Fact]
        public void NotEqualTo()
        {
            Assume.
                That("actual").
                Is.Not.Equal.To("notactual");
                
            try
            {
                Assume.
                    That("actual").
                    Is.Not.Equal.To("actual");

                throw new Exception("Assumption was not thrown");
            }
            catch (AssumptionFailure ex)
            {
                Assert.Equal("Expected actualValue (actual) to not be equal to expectedValue (actual)", ex.Message);
            }
        }
        
        [Fact]
        public void Not()
        {
            var that = Assume.That("asdf");
            var thatNot = that.Not;
            Assert.Same(that, thatNot);
        }
        
        [Fact]
        public void NotLessThan()
        {
            Assume.
                That(1).
                Is.Not.Less.Than(0);
                
            Assume.
                That(1).
                Is.Not.Less.Than(1);
            
            try
            {
                Assume.
                    That(1).
                    Is.Not.Less.Than(2);

                throw new Exception("Assumption was not thrown");
            }
            catch (AssumptionFailure ex)
            {
                Assert.Equal("Expected actualValue (1) to not be less than expectedValue (2)", ex.Message);
            }
        }
        
        [Fact]
        public void NotLessThanOrEqualTo()
        {
            Assume.
                That(2).
                Is.Not.LessThanOrEqual.To(1);
                
            try
            {
                Assume.
                    That(1).
                    Is.Not.LessThanOrEqual.To(1);

                throw new Exception("Assumption was not thrown");
            }
            catch (AssumptionFailure ex)
            {
                Assert.Equal("Expected actualValue (1) to not be less than, or equal to, expectedValue (1)", ex.Message);
            }
                        
            try
            {
                Assume.
                    That(1).
                    Is.Not.LessThanOrEqual.To(2);

                throw new Exception("Assumption was not thrown");
            }
            catch (AssumptionFailure ex)
            {
                Assert.Equal("Expected actualValue (1) to not be less than, or equal to, expectedValue (2)", ex.Message);
            }
        }
        
        [Fact]
        public void NotGreaterThan()
        {
            Assume.
                That(1).
                Is.Not.Greater.Than(2);
                
            Assume.
                That(1).
                Is.Not.Greater.Than(1);
            
            try
            {
                Assume.
                    That(2).
                    Is.Not.Greater.Than(1);

                throw new Exception("Assumption was not thrown");
            }
            catch (AssumptionFailure ex)
            {
                Assert.Equal("Expected actualValue (2) to not be greater than expectedValue (1)", ex.Message);
            }
        }
        
        [Fact]
        public void NotGreaterThanOrEqualTo()
        {
            Assume.
                That(1).
                Is.Not.GreaterThanOrEqual.To(2);
                
            try
            {
                Assume.
                    That(1).
                    Is.Not.GreaterThanOrEqual.To(1);

                throw new Exception("Assumption was not thrown");
            }
            catch (AssumptionFailure ex)
            {
                Assert.Equal("Expected actualValue (1) to not be greater than, or equal to, expectedValue (1)", ex.Message);
            }
            
            try
            {
                Assume.
                    That(2).
                    Is.Not.GreaterThanOrEqual.To(1);

                throw new Exception("Assumption was not thrown");
            }
            catch (AssumptionFailure ex)
            {
                Assert.Equal("Expected actualValue (2) to not be greater than, or equal to, expectedValue (1)", ex.Message);
            }
        }
        
        [Fact]
        public void NotTrue()
        {
            Assume.
                That(false).
                Is.Not.True();
                
            try
            {
                Assume.
                    That(true).
                    Is.Not.True();

                throw new Exception("Assumption was not thrown");
            }
            catch (AssumptionFailure ex)
            {
                Assert.Equal("Expected actualValue (True) to not be True", ex.Message);
            }
        }
        
        [Fact]
        public void NotFalse()
        {
            Assume.
                That(true).
                Is.Not.False();
                
            try
            {
                Assume.
                    That(false).
                    Is.Not.False();

                throw new Exception("Assumption was not thrown");
            }
            catch (AssumptionFailure ex)
            {
                Assert.Equal("Expected actualValue (False) to not be False", ex.Message);
            }
        }
        
        [Fact]
        public void NotNull()
        {
            Assume.
                That("asdf").
                Is.Not.Null();
                
            try
            {
                Assume.
                    That(null).
                    Is.Not.Null();

                throw new Exception("Assumption was not thrown");
            }
            catch (AssumptionFailure ex)
            {
                Assert.Equal("Expected actualValue (<null>) to not be null", ex.Message);
            }
        }
    }
}
