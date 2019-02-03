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
                Assert.Equal("Expected 'actual' to be equal to 'notactual'", ex.Message);
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
                Assert.Equal("Expected '1' to be less than '1'", ex.Message);
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
                Assert.Equal("Expected '2' to be less than '1'", ex.Message);
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
                Assert.Equal("Expected '2' to be less than or equal to '1'", ex.Message);
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
                Assert.Equal("Expected '1' to be greater than '1'", ex.Message);
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
                Assert.Equal("Expected '1' to be greater than '2'", ex.Message);
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
                Assert.Equal("Expected '1' to be greater than or equal to '2'", ex.Message);
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
                Assert.Equal("Expected <false> to be <true>", ex.Message);
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
                Assert.Equal("Expected <true> to be <false>", ex.Message);
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
                Assert.Equal("Expected 'asdf' to be <null>", ex.Message);
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
                Assert.Equal($"Expected System.Int32 to be derived from System.Exception", ex.Message);
            }
            
            
            Assume.That(1).Is.InstanceOf(new []{ typeof(Exception),typeof(Int32) });
            
            try
            {
                Assume.That(1).Is.InstanceOf(new []{ typeof(Exception),typeof(Decimal) });
                
                throw new Exception("Assumption was not thrown");
            }
            catch (AssumptionFailure ex)
            {
                Assert.Equal($"Expected System.Int32 to be derived from one of the following: System.Exception, System.Decimal", ex.Message);
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
                Assume.That(ex).Is.InstanceOf(new []{ typeof(NotSupportedException), typeof(AssumptionFailure) });
            }
            
            try
            {
                Assume.That(true).Is.False();

                throw new Exception("Assumption was not thrown");
            }
            catch (Exception ex)
            {
                Assume.That(ex).Is.InstanceOf(new []{ typeof(NotSupportedException), typeof(AssumptionFailure) });
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
                    Assert.Equal("Expected System.NotSupportedException to be derived from Assumptions.AssumptionFailure", ex2.Message);
                }
            }
        }
        
        [Fact]
        public void Empty()
        {
            Assume.
                That("").
                Is.Empty();
                
            try
            {
                Assume.
                    That("asdf").
                    Is.Empty();

                throw new Exception("Assumption was not thrown");
            }
            catch (AssumptionFailure ex)
            {
                Assert.Equal("Expected 'asdf' to be empty", ex.Message);
            }
            
            Assume.
                That(new int[0]).
                Is.Empty();
                
            try
            {
                Assume.
                    That(new []{ "asdf" }).
                    Is.Empty();

                throw new Exception("Assumption was not thrown");
            }
            catch (AssumptionFailure ex)
            {
                Assert.Equal("Expected collection to be empty", ex.Message);
            }
        }

        [Fact]
        public void And()
        {
            Assume.That(1).
                Greater.Than(0).
                And.GreaterThanOrEqual.To(-1).
                And.Less.Than(2);
                
            try
            {
                Assume.That(1).
                    Greater.Than(2).
                    And.GreaterThanOrEqual.To(-1).
                    And.Less.Than(2);
                throw new Exception("Assumption was not thrown");
            }
            catch (AssumptionFailure)
            {
            }
            
            try
            {
                Assume.That(1).
                    Greater.Than(0).
                    And.GreaterThanOrEqual.To(11).
                    And.Less.Than(2);
                throw new Exception("Assumption was not thrown");
            }
            catch (AssumptionFailure)
            {
            }
            
            try
            {
                Assume.That(1).
                    Greater.Than(2).
                    And.GreaterThanOrEqual.To(-1).
                    And.Less.Than(-2222);
                throw new Exception("Assumption was not thrown");
            }
            catch (AssumptionFailure)
            {
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
                Assert.Equal("Expected lambda to run to completion without raising an exception", ex.Message);
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
                Assert.Equal("Expected lambda to raise an exception before running to completion", ex.Message);
            }
        }
        
        [Fact]
        public void CanProvideExplanation()
        {
            try
            {
                Assume.That(1).Is.Not.Equal.To(1, "The number of foos must not equal the number of bars");
                
                throw new Exception("Assumption was not thrown");
            }
            catch (AssumptionFailure ex)
            {
                Assert.Equal("The number of foos must not equal the number of bars. Expected '1' to not be equal to '1'", ex.Message);
            }
            
            try
            {
                Assume.That(3).Is.Less.Than(2, "The number of blats must be less than the number of splats");
                
                throw new Exception("Assumption was not thrown");
            }
            catch (AssumptionFailure ex)
            {
                Assert.Equal("The number of blats must be less than the number of splats. Expected '3' to be less than '2'", ex.Message);
            }

            try
            {
                var lifeIsPeachy = true;
                Assume.That(lifeIsPeachy).Is.Not.True("Life is like a box of chocolates");
                
                throw new Exception("Assumption was not thrown");
            }
            catch (AssumptionFailure ex)
            {
                Assert.Equal("Life is like a box of chocolates. Expected <true> to not be <true>", ex.Message);
            }

            try
            {
                var lifeIsPeachy = true;
                Assume.That(lifeIsPeachy).Is.False("Life is like a box of chocolates. You never known what you're going to get");
                
                throw new Exception("Assumption was not thrown");
            }
            catch (AssumptionFailure ex)
            {
                Assert.Equal("Life is like a box of chocolates. You never known what you're going to get. Expected <true> to be <false>", ex.Message);
            }

            try
            {
                Assume.That(null).Is.Not.Null("To be, or not to be... null...");
                
                throw new Exception("Assumption was not thrown");
            }
            catch (AssumptionFailure ex)
            {
                Assert.Equal("To be, or not to be... null.... Expected <null> to not be <null>", ex.Message);
            }

            try
            {
                Assume.That("").Is.Not.Empty("To be, or not to be... empty...");
                
                throw new Exception("Assumption was not thrown");
            }
            catch (AssumptionFailure ex)
            {
                Assert.Equal("To be, or not to be... empty.... Expected '' to not be empty", ex.Message);
            }

            try
            {
                Assume.That(new NotSupportedException()).Is.Not.InstanceOf(new []{ typeof(NotSupportedException) },
                    "We should not get a not-supported-exception thrown into our face");
                
                throw new Exception("Assumption was not thrown");
            }
            catch (AssumptionFailure ex)
            {
                Assert.Equal("We should not get a not-supported-exception thrown into our face. Expected System.NotSupportedException to not be derived from System.NotSupportedException", ex.Message);
            }
            
            try
            {
                Assume.That(new ArgumentNullException()).Is.InstanceOf(new []{ typeof(NotSupportedException) },
                    "We should get a not-supported-exception thrown into our face");
                
                throw new Exception("Assumption was not thrown");
            }
            catch (AssumptionFailure ex)
            {
                Assert.Equal("We should get a not-supported-exception thrown into our face. Expected System.ArgumentNullException to be derived from System.NotSupportedException", ex.Message);
            }
        }
    }
}
