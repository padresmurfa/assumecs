using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Security;
using Xunit;
using Assumptions;
using Assumptions.Memory;
using Microsoft.DotNet.PlatformAbstractions;

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


        private void ApplyChaos(
            Action<int,SourceCodeLocation> action,
            SourceCodeLocation location)
        {
            var random = new Random(123456789);

            for (var i = 0; i < 10000; i++)
            {
                var chaos = random.Next() % 100;

                action(chaos, location);
            }
        }

        void ChaoticallyFalse(
            Action<int,SourceCodeLocation> action,
            string callerId = null,
            [CallerMemberName] string callerMemberName = "",
            [CallerFilePath] string callerSourceFilePath = "",
            [CallerLineNumber] int callerSourceLineNumber = 0)
        {
            callerId = (callerId ?? string.Empty) + "-failed";
            
            var location = new SourceCodeLocation(callerId, callerMemberName, callerSourceFilePath, callerSourceLineNumber);
            
            try
            {
                ApplyChaos(action, location);
                Assert.True(false);
            }
            catch (AssumptionFailure)
            {
            }
        }

        void ChaoticallyTrue(
            Action<int,SourceCodeLocation> action,
            string callerId = null,
            [CallerMemberName] string callerMemberName = "",
            [CallerFilePath] string callerSourceFilePath = "",
            [CallerLineNumber] int callerSourceLineNumber = 0)
        {
            callerId = (callerId ?? string.Empty) + "-succeeded";
            
            var location = new SourceCodeLocation(callerId, callerMemberName, callerSourceFilePath, callerSourceLineNumber);
            
            ApplyChaos(action, location);
        }
        
        [Fact]
        public void CanDeclareThatSomethingIsProbablyTheCase()
        {
            ChaoticallyTrue( (actual, location) =>
            {
                Assume.That(actual, location.Here()).
                    Is.Probably(0.95f, 10).
                    Not.Greater.
                    Than(95,  $"Expected random d100's to usually not be greater than 95");
            });
                    
            ChaoticallyFalse( (actual, location) =>
            {
                Assume.That(actual, location.Here()).
                    Is.Probably(0.95f, 10).
                    Less.
                    Than(85,  $"Expected random d100's to NOT usually be less than 85");
            });
        }    
    
        [Fact]
        public void CanDeclareThatSomethingIsCertainlyTheCase()
        {
            ChaoticallyTrue( (actual, location) =>
            {
                Assume.That(actual, location.Here()).
                    Is.Certainly(10).
                    Not.GreaterThanOrEqual.
                    To(99,  $"Expected random d100's to usually be statistically above 99");
                
                Assume.That(actual, location.Here()).
                    Is.Certainly(10).
                    GreaterThanOrEqual.
                    To(1,  $"Expected random d100's to usually be statistically above 99");
            });
                    
            ChaoticallyFalse( (actual, location) =>
            {
                Assume.That(actual, location.Here()).
                    Is.Certainly(10).
                    Not.GreaterThanOrEqual.
                    To(1,  $"Expected random d100's to usually be statistically above 1");
            });
        }
        
        [Fact]
        public void CanDeclareThatSomethingIsCertainlyNotTheCase()
        {
            ChaoticallyTrue( (actual, location) =>
            {
                Assume.That(actual, location.Here()).
                    Is.CertainlyNot(10).
                    LessThanOrEqual.
                    To(1,  $"Expected random d100's to usually be statistically above 1");
            });
                    
            ChaoticallyFalse( (actual, location) =>
            {
                Assume.That(actual, location.Here()).
                    Is.CertainlyNot(10).
                    LessThanOrEqual.
                    To(99,  $"Expected random d100's to usually be statistically above 99");
            });
        }
        
        [Fact]
        public void CanDeclareThatSomethingIsConsistent()
        {
            var r = new Random(43838);
            for (var i = 0; i < 10; i++)
            {
                var probability = r.Next() % 100;

                ChaoticallyTrue((actual, location) =>
                {
                    Assume.That(actual, location.Here(i.ToString())).Is.Consistently(50).Greater.Than(probability,
                        $"Expected random d100's to usually be greater than {probability}");
                });
            }

            var n = 0;
            var x = 50;
            
            ChaoticallyFalse((actual, location) =>
            {
                if (n++ % 18 == 0)
                {
                    x = r.Next();
                }

                Assume.That(actual, location.Here()).Is.Consistently(50).Greater
                    .Than(x, $"Expected random d100's to usually be greater than {x}");
            });
        }

        [Fact]
        public void CanDetectMemoryLeak()
        {
            Assume.That(() =>
            {
                var g = new {i = 0};
            }).Does.Not.Leak();

            var leak = new List<object>();
            Assume.That(() =>
            {
                leak.Add(new {i = 0});
            }).Leaks();
        }
        
        [Fact]
        public void CanDetectMemoryAllocations()
        {
            Assume.That(() =>
            {
                var g = new {i = 0};
            }).Allocates.LessThanOrEqual.To( RAM.FromBytes(100L) );

            var leak = new List<object>();
            Assume.That(() =>
            {
                for (var i = 0; i < 10000; i++)
                {
                    leak.Add(new {i = 0});
                }
            }).Allocates.More.Than( RAM.FromBytes(1000L) );
        }
        
        /*
         
            // TODO: "allocates" is a resource usage assumption.  It can also apply to "takes less than 10 seconds",
            //       or some other such measure.  Uses less than 2 cpu seconds, sends less than 200 bytes of data,
            //       etc.
            
         assume consistent 
         * persistent storage to determine relationship between tests, or just over time
         
         what assumptions have not been tested?
         
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
        Anything else should probably always be strict by default.
        
        ----
        
        testing - autodetect low-value tests, run tests in priority queue with blocking bridge.

        // or that something correlates e.g. linearly to something else
                 
        
        [Fact]
        public void CanDeclareThatSomethingBehavesConsistentlyOverTime()
        {
            // similar to consistent over invocations, except using time instead of invocation count to
            // determine the distance between samples to compare.
            // may also want to use a persistent store here.
        }
                 
        [Fact]
        public void CanDeclareThatSomethingObeysATemporalConstraint()
        {
            Assume.That(()=>{
                Thread.Sleep(10000);
            }).Completes.In.Less.Than(TimeSpan.FromSeconds(3));
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
        
        
        
public static class CodeFirst
{
    public string GetTestMethodName()
    {
        try
        {
            // for when it runs via TeamCity
            return TestContext.CurrentContext.Test.Name;
        }
        catch
        {
            // for when it runs via Visual Studio locally
            var stackTrace = new StackTrace(); 
            foreach (var stackFrame in stackTrace.GetFrames())
            {
                MethodBase methodBase = stackFrame.GetMethod();
                Object[] attributes = methodBase.GetCustomAttributes(
                                          typeof(TestAttribute), false); 
                if (attributes.Length >= 1)
                {
                    return methodBase.Name;
                }
            }
            return "Not called from a test method";  
        }
    }

    namespace UnitTests
    {
        public class Blark
        {
            private static CodeFirst CodeFirst;
            
            private void Foo()
            {
                CodeFirst.Is.Covered;
            }
        
            private void Blat()
            {
                var code = "asdf";
                ...
                
                switch (CodeFirst.Covers(code))
                {
                    case "asdf":
                        // do some asdf stuff
                        break;
                        
                    default:
                        // do something else
                }
            }
        
            private void Splat()
            {
                CodeFirst.Covers(true)
            }
        
            private void Flat()
            {
                if (CodeFirst.Covers(false))
                {
                }
            }
            
            // TODO: CodeFirst.Covers should be syntactic sugar for
            // Assume.That(CodeFirst).Covers.
        }
        
        [Covers(typeof(Blark)]
        public class BlarkTests
        {
            // TODO: make the attribute, when constructed, create a coverage declaration.
            //       if there are any coverage declarations, then we are in a test build
            //       and if we are in a test build, then each Assume.Tested can perform a
            //       lookup involving a stack walk if need be.  It will be slow, but it
            //       might be ok.
            [Fact]
            [Covers(nameof(Blark.Foo))]
            [Covers(nameof(Blark.Blat), "asdf")]
            [Covers(nameof(Blark.Splat), true)]
            public void CanTestCoverage()
            {
                Foo();
                Blat();
                Splat();
            
                // Not covered.
                Flat();
            }
        }
        */
    }
}
















