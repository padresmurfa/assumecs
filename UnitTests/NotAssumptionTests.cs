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
                Assert.Equal("Expected 'actual' to not be equal to 'actual'", ex.Message);
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
                Assert.Equal("Expected '1' to not be less than '2'", ex.Message);
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
                Assert.Equal("Expected '1' to not be less than or equal to '1'", ex.Message);
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
                Assert.Equal("Expected '1' to not be less than or equal to '2'", ex.Message);
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
                Assert.Equal("Expected '2' to not be greater than '1'", ex.Message);
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
                Assert.Equal("Expected '1' to not be greater than or equal to '1'", ex.Message);
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
                Assert.Equal("Expected '2' to not be greater than or equal to '1'", ex.Message);
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
                Assert.Equal("Expected <true> to not be <true>", ex.Message);
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
                Assert.Equal("Expected <false> to not be <false>", ex.Message);
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
                Assert.Equal("Expected <null> to not be <null>", ex.Message);
            }
        }
        
        [Fact]
        public void NotEmpty()
        {
            Assume.
                That("asdf").
                Is.Not.Empty();
                
            try
            {
                Assume.
                    That(null).
                    Is.Not.Empty();

                throw new Exception("Assumption was not thrown");
            }
            catch (AssumptionFailure ex)
            {
                Assert.Equal("Expected '' to not be empty", ex.Message);
            }
            
            Assume.
                That(new System.Collections.Generic.List<bool> { true, false}).
                Is.Not.Empty();
                
            try
            {
                Assume.
                    That(new System.Collections.Generic.HashSet<string> {}).
                    Is.Not.Empty();

                throw new Exception("Assumption was not thrown");
            }
            catch (AssumptionFailure ex)
            {
                Assert.Equal("Expected collection to not be empty", ex.Message);
            }
        }
    }
}
