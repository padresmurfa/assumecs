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
                Is.Equal.To("actual");
                
            try
            {
                Assume.
                    That("actual").
                    Is.Equal.To("notactual");

                throw new Exception("Assumption was not thrown");
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
                Is.Less.Than(2);
                
            try
            {
                Assume.
                    That(1).
                    Is.Less.Than(1);

                throw new Exception("Assumption was not thrown");
            }
            catch (AssumptionFailure ex)
            {
                Assert.Equal("Expected actualValue (1) to be less than expectedValue (1)", ex.Message);
            }
            
            try
            {
                Assume.
                    That(2).
                    Is.Less.Than(1);

                throw new Exception("Assumption was not thrown");
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
                Is.LessThanOrEqual.To(2);
                
            Assume.
                That(1).
                Is.LessThanOrEqual.To(1);
            
            try
            {
                Assume.
                    That(2).
                    Is.LessThanOrEqual.To(1);

                throw new Exception("Assumption was not thrown");
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
                Is.Greater.Than(1);
                
            try
            {
                Assume.
                    That(1).
                    Is.Greater.Than(1);

                throw new Exception("Assumption was not thrown");
            }
            catch (AssumptionFailure ex)
            {
                Assert.Equal("Expected actualValue (1) to be greater than expectedValue (1)", ex.Message);
            }
            
            try
            {
                Assume.
                    That(1).
                    Is.Greater.Than(2);

                throw new Exception("Assumption was not thrown");
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
                Is.GreaterThanOrEqual.To(1);
                
            Assume.
                That(1).
                Is.GreaterThanOrEqual.To(1);
            
            try
            {
                Assume.
                    That(1).
                    Is.GreaterThanOrEqual.To(2);

                throw new Exception("Assumption was not thrown");
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

                throw new Exception("Assumption was not thrown");
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

                throw new Exception("Assumption was not thrown");
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

                throw new Exception("Assumption was not thrown");
            }
            catch (AssumptionFailure ex)
            {
                Assert.Equal("Expected actualValue (asdf) to be null", ex.Message);
            }
        }
        
        [Fact]
        public void InstanceOf()
        {
            Assume.That(new Exception()).Is.InstanceOf(typeof(Exception));
            
            Assume.That(new AssumptionFailure("asdf",null,"mem","pat",1)).Is.InstanceOf(typeof(Exception));
            
            try
            {
                Assume.That(1).Is.InstanceOf(typeof(Exception));

                throw new Exception("Assumption was not thrown");
            }
            catch (AssumptionFailure ex)
            {
                Assert.Equal($"Expected actualValue (System.Int32) to be derived from System.Exception", ex.Message);
            }
            
            
            Assume.That(1).Is.InstanceOf(typeof(Exception),typeof(Int32));
            
            try
            {
                Assume.That(1).Is.InstanceOf(typeof(Exception),typeof(Decimal));
                
                throw new Exception("Assumption was not thrown");
            }
            catch (AssumptionFailure ex)
            {
                Assert.Equal($"Expected actualValue (System.Int32) to be derived from one of the following: System.Exception, System.Decimal", ex.Message);
            }
            
        }
        
        [Fact]
        public void CanMakeAssumptionsAboutExceptionHandling()
        {
            try
            {
                throw new NotSupportedException("bork");
            }
            catch (Exception ex)
            {
                Assume.That(ex).Is.InstanceOf(typeof(NotSupportedException), typeof(AssumptionFailure));
            }
            
            try
            {
                Assume.That(true).Is.False();

                throw new Exception("Assumption was not thrown");
            }
            catch (Exception ex)
            {
                Assume.That(ex).Is.InstanceOf(typeof(NotSupportedException), typeof(AssumptionFailure));
            }
            
            try
            {
                throw new NotSupportedException("bork");
            }
            catch (Exception ex)
            {
                try
                {
                    Assume.That(ex).Is.InstanceOf(typeof(AssumptionFailure));
                    
                    throw new Exception("Assumption was not thrown");
                }
                catch (AssumptionFailure ex2)
                {
                    Assert.Equal("Expected actualValue (System.NotSupportedException) to be derived from Assumptions.AssumptionFailure", ex2.Message);
                }
            }
        }
        
        [Fact]
        public void CanMakeAssumptionsAboutThrowingExceptions()
        {
            Assume.That(() => {
                var i = 0;
                i++;
            }).Completed();
            
            try
            {
                Assume.That(() => {
                    throw new Exception("foo");
                }).Completed();
                
                throw new Exception("Assumption was not thrown");
            }
            catch (AssumptionFailure ex)
            {
                Assert.Equal("Expected actualValue to run to completion without raising an exception", ex.Message);
            }
            
            try
            {
                Assume.That(() => {
                    throw new Exception("foo");
                }).Not.Completed();
                
                throw new Exception("Assumption was not thrown");
            }
            catch (Exception ex)
            {
                Assert.Equal("foo", ex.Message);
            }
            
            try
            {
                Assume.That(() => {
                    var i = 0;
                    i++;
                }).Not.Completed();
                
                throw new Exception("Assumption was not thrown");
            }
            catch (AssumptionFailure ex)
            {
                Assert.Equal("Expected actualValue to raise an exception before running to completion", ex.Message);
            }
        }
    }
}
