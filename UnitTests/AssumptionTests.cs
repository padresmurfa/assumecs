using System;
using System.Threading;
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
            
            var callerId = new SourceCodeLocation("id", "member", "path", 1234);
            
            Assume.That(new AssumptionFailure("asdf",null, callerId)).Is.InstanceOf(typeof(Exception));
            
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

        [Fact]
        public void CanDeclareThatSomethingIsProbablyTheCase()
        {
            var random = new Random(123456789);

            void test(int expected, SourceCodeLocation location)
            {
                for (var i = 0; i < 10000; i++)
                {
                    var assumeThatActualValue = Assume.That(random.Next() % 100, location.Here());
                    
                    assumeThatActualValue.
                        Is.Inconceivably(1.0f - 0.95f, 10).
                        GreaterThanOrEqual.
                        To(100 - expected,  $"Expected random d100's to usually be statistically above {expected}");
                    
                    assumeThatActualValue.
                        Is.Probably(0.95f, 10).
                        LessThanOrEqual.
                        To(expected,  $"Expected random d100's to usually be statistically above {expected}");
                    
                    assumeThatActualValue.
                        Is.Probably(0.95f, 10).
                        Not.Greater.
                        Than(expected,  $"Expected random d100's to usually be statistically above {expected}");
                    
                    assumeThatActualValue.
                        Is.Sometimes(1.0f - 0.95f, 10).
                        GreaterThanOrEqual.
                        To(100 - expected,  $"Expected random d100's to usually be statistically above {expected}");
                    
                    assumeThatActualValue.
                        Is.Certainly(0.95f, 10).
                        Not.GreaterThanOrEqual.
                        To(expected,  $"Expected random d100's to usually be statistically above {expected}");
                    
                    assumeThatActualValue.
                        Is.Occasionally(1.0f - 0.95f, 10).
                        Not.Less.
                        Than(100 - expected,  $"Expected random d100's to usually be statistically above {expected}");
                    
                    // TODO: consider creating a good syntax for probably-not.  The actual probability is easy, but the
                    // expected value is perhaps unsolvable using nice syntax, and the usability is perhaps weird.
                    
                    // TODO: certainly and inconceivably should have a default probability of 100 and 0, respectively
                }
            }

            // this test should fail, since 85% is obviously not statistically above 95%, unless we have a really poor
            // sample, which is not very likely given the total and minimum sample size
            try
            {
                test(85, new SourceCodeLocation("CanDeclareThatSomethingIsUsuallyTheCase.failure"));
                Assert.True(false);
            }
            catch (AssumptionFailure)
            {
            }
            
            // and this test should succeed, since 95% is obviously statistically speaking above (if only a fraction) 95%
            // given any sort of sane sample.
            test(95, new SourceCodeLocation("CanDeclareThatSomethingIsUsuallyTheCase.success"));
        }

        /*
        
        minimum sample size could be determined from a global default.  I.e. we don't trust sample sizes that imply less
        than x% certainty.  Not sure that this is possible...
        //
        // I think it can be done if you assume e.g. a normal distribution
        //
        // https://www.qualtrics.com/experience-management/research/determine-sample-size/
        // https://www.dummies.com/education/math/statistics/how-to-determine-the-minimum-size-needed-for-a-statistical-sample/
        //
        // perhaps it can be done with others if a distribution is specified and formula used for it as well:
        // https://blog.cloudera.com/blog/2015/12/common-probability-distributions-the-data-scientists-crib-sheet/
        //
        // perhaps one of the cool things to assume would be that a probability follows a particular distribution.
        
        ---
        strict mode vs relaxed mode assumptions:
        
        Basically, what are the consequences of an assumption failure?
        -> Log?
        -> Metrics?
        -> Exception?
        
        Is there any value in being able to programmatically state that
        something is strictly the case vs the case in a relaxed fashion?
        
        Probably should probably never be strict by default.
        Anything else hitherto specified should probably always be strict by default.
        
        ----
        
        testing - autodetect low-value tests, run tests in priority queue with blocking bridge.
        
        [Fact]
        public void CanTILAB()
        {
            var tilab = true;
            
            Assume.That(tilab).
                Is.Covered.By("can_test_it_like_a_boss").
                And.That(!x).Is.Covered.By("!can_test_it_like_a_boss");

            if (TDD.Cover(tilab,"can_test_it_like_a_boss"))
            {
            }
            
            TDD.Cover("reached")
            
            TDD.Covers("#can_test_it_like_a_boss");
        }
        
        [Fact]
        public void CanDeclareThatSomethingBehavesConsistentlyOverTime()
        {
            this can be modelled, e.g. by having two accumulated probability functions,
            
            one starts immediately (A)
            the other (B) after e.g. 30 minutes or 20 occurrances (basically as determined by the consistency-interval)
            
            the assertion is then that B could be equal to A.
            if this holds true, then don't go apeshit.
            
            This can either be a boolean, or a number.
        
            var hit = 85;
            var random = new Random();
            for (var i = 0; i < 10000; i++)
            {
                var d100 = random.Next() % 100;
                if (i > 1000)
                {
                    // should be detected
                    d100 /= 2;
                }
                Assume.
                    That(d100).
                    Is.Consistently.Over.The.Last(10).Occurrances.
                    LessThanOrEqual.To(hit);
            }
        }
                 
        // or that something correlates e.g. linearly to something else
                 
        [Fact]
        public void CanDeclareThatSomethingObeysATemporalConstraint()
        {
            Assume.That(()=>{
                Thread.Sleep(10000);
            }).Completes.In.Less.Than(TimeSpan.FromSeconds(3));
        }

        [Fact]
        public void CanDeclareThatSomethingObeysAPermanentSpatialConstraint()
        {
            byte[] v;
            Assume.That(()=>{
                v = new byte[100000];
            }).Completes.Retaining.Less.Than(Resources.RAM.Kilobytes(80));
        }

        [Fact]
        public void CanDeclareThatSomethingObeysATemporalConstraint()
        {
            Assume.That(()=>{
                Thread.Sleep(10000);
            }).Completes.Using.Less.Than(Resources.CPU(TimeSpan.FromMilliseconds(500)));
        }
        */

        /*
        [Fact]
        public void CanDeclareThatSomethingObeysATemporarySpatialConstraint()
        {
            Assume.That(()={
                var v = new byte[100000];
            }).Completes.Allocating.Less.Than(Resources.RAM.Kilobytes(80));
        }
        */
    }
}
















