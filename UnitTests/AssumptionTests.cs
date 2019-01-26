using System;
using Xunit;
using Assumptions;

namespace UnitTests
{
    public class AssumptionTests
    {
        [Fact]
        public void EqualTo()
        {
            Assume.
                That("actual").
                Equal.To("actual");
                
            try
            {
                Assume.
                    That("actual").
                    Equal.To("notactual");
            }
            catch (AssumptionFailure ex)
            {
                Assert.Equal("Expected actualValue (actual) to be equal to expectedValue (notactual)", ex.Message);
            }
        }
        
        [Fact]
        public void Is()
        {
            var that = Assume.That("asdf");
            var thatIs = that.Is;
            Assert.Same(that, thatIs);
        }
        
        [Fact]
        public void LessThan()
        {
            Assume.
                That(1).
                Less.Than(2);
                
            try
            {
                Assume.
                    That(1).
                    Less.Than(1);
            }
            catch (AssumptionFailure ex)
            {
                Assert.Equal("Expected actualValue (1) to be less than expectedValue (1)", ex.Message);
            }
            
            try
            {
                Assume.
                    That(2).
                    Less.Than(1);
            }
            catch (AssumptionFailure ex)
            {
                Assert.Equal("Expected actualValue (2) to be less than expectedValue (1)", ex.Message);
            }
        }
        
        [Fact]
        public void LessThanOrEqualTo()
        {
            Assume.
                That(1).
                LessOrEqual.To(2);
                
            Assume.
                That(1).
                LessOrEqual.To(1);
            
            try
            {
                Assume.
                    That(2).
                    LessOrEqual.To(1);
            }
            catch (AssumptionFailure ex)
            {
                Assert.Equal("Expected actualValue (2) to be less than, or equal to, expectedValue (1)", ex.Message);
            }
        }
        
        [Fact]
        public void GreaterThan()
        {
            Assume.
                That(2).
                Greater.Than(1);
                
            try
            {
                Assume.
                    That(1).
                    Greater.Than(1);
            }
            catch (AssumptionFailure ex)
            {
                Assert.Equal("Expected actualValue (1) to be greater than expectedValue (1)", ex.Message);
            }
            
            try
            {
                Assume.
                    That(1).
                    Greater.Than(2);
            }
            catch (AssumptionFailure ex)
            {
                Assert.Equal("Expected actualValue (1) to be greater than expectedValue (2)", ex.Message);
            }
        }
        
        [Fact]
        public void GreaterThanOrEqualTo()
        {
            Assume.
                That(2).
                GreaterOrEqual.To(1);
                
            Assume.
                That(1).
                GreaterOrEqual.To(1);
            
            try
            {
                Assume.
                    That(1).
                    GreaterOrEqual.To(2);
            }
            catch (AssumptionFailure ex)
            {
                Assert.Equal("Expected actualValue (1) to be greater than, or equal to, expectedValue (2)", ex.Message);
            }
        }
        
        [Fact]
        public void True()
        {
            Assume.
                That(true).
                Is.True();
                
            try
            {
                Assume.
                    That(false).
                    Is.True();
            }
            catch (AssumptionFailure ex)
            {
                Assert.Equal("Expected actualValue (False) to be True", ex.Message);
            }
        }
        
        [Fact]
        public void False()
        {
            Assume.
                That(false).
                Is.False();
                
            try
            {
                Assume.
                    That(true).
                    Is.False();
            }
            catch (AssumptionFailure ex)
            {
                Assert.Equal("Expected actualValue (True) to be False", ex.Message);
            }
        }
        
        [Fact]
        public void Null()
        {
            Assume.
                That(null).
                Is.Null();
                
            try
            {
                Assume.
                    That("asdf").
                    Is.Null();
            }
            catch (AssumptionFailure ex)
            {
                Assert.Equal("Expected actualValue (asdf) to be null", ex.Message);
            }
        }
        
        [Fact]
        public void NotNull()
        {
            Assume.
                That("asdf").
                Is.NotNull();
                
            try
            {
                Assume.
                    That(null).
                    Is.NotNull();
            }
            catch (AssumptionFailure ex)
            {
                Assert.Equal("Expected actualValue (<null>) to not be null", ex.Message);
            }
        }
        
        [Fact(Skip = "Not yet implemented")]
        public void CanUseNot()
        {
            /*
            Assume.
                That(true).
                Is.Not.False(false);
            */
        }
        
        [Fact(Skip = "Not yet implemented")]
        public void CanUseThrows()
        {
            /*
            Assume.
                That(()=>{
                    throw new Exception("asdf");
                }).
                Throws(typeof(Exception));
                
            Assume.
                That(()=>{
                    throw new Exception("asdf");
                }).
                DoesNot.Throw();
            */
        }
    }
}
