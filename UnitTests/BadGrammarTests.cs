using System;
using Xunit;
using Assumptions;

namespace UnitTests
{
    public class BadGrammarTests
    {
        [Fact]
        public void DanglingNot()
        {
            try
            {
                Assume.
                    That("actual").
                    Is.Equal.Not.To("actual");

                throw new Exception("Assumption was not thrown");
            }
            catch (BadGrammar ex)
            {
                Assert.Equal("Not must be specified before a logical operator, not before a terminator", ex.Message);
            }
        }
        
        [Fact]
        public void IgnoreUsingToInsteadOfThan()
        {
            Assume.That(1).Is.Less.To(2);
        }
        
        [Fact]
        public void IgnoreUsingThanInsteadOfTo()
        {
            Assume.That("asdf").Is.Equal.Than("asdf");
        }
        
        [Fact]
        public void IgnoreOmittedIs()
        {
            Assume.That(1).Less.To(2);
        }

        [Fact(Skip = "Haven't found a way to implement this test, syntactically")]
        public void DanglingComparator()
        {
            try
            {
                var l = Assume.That(1).Less;

                throw new Exception("Assumption was not thrown");
            }
            catch (BadGrammar ex)
            {
                Assert.Equal("Comparators must use two arguments (actual and expected)", ex.Message);
            }
        }
        
        [Fact]
        public void And()
        {
            try
            {
                var l = Assume.That(1).And;
                throw new Exception("Assumption was not thrown");
            }
            catch (BadGrammar)
            {
            }
            
            try
            {
                var v = Assume.That(1).Greater.Than(0).And.And;
                throw new Exception("Assumption was not thrown");
            }
            catch (BadGrammar)
            {
            }
        }
        
        [Fact]
        public void MultipleOperators()
        {
            Func<Assumption,Assumption>[] assumptions = new Func<Assumption,Assumption>[]{
                (a) => a.Less.Equal,
                (a) => a.Less.Less,
                (a) => a.Less.LessThanOrEqual,
                (a) => a.Less.Greater,
                (a) => a.Less.GreaterThanOrEqual,
                (a) => a.Less.False(),
                (a) => a.Less.True(),
                (a) => a.Less.Null(),
                
                (a) => a.Less.Not.Equal,
                (a) => a.Less.Not.Less,
                (a) => a.Less.Not.LessThanOrEqual,
                (a) => a.Less.Not.Greater,
                (a) => a.Less.Not.GreaterThanOrEqual,
                (a) => a.Less.Not.False(),
                (a) => a.Less.Not.True(),
                (a) => a.Less.Not.Null(),
                
                (a) => a.Equal.Less,
                (a) => a.Less.Less,
                (a) => a.LessThanOrEqual.Less,
                (a) => a.Greater.Less,
                (a) => a.GreaterThanOrEqual.Less
            };
            
            foreach (var af in assumptions)
            {
                try
                {
                    var assumption = af(Assume.That("asdf").Is);
    
                    throw new Exception("Assumption was not thrown");
                }
                catch (BadGrammar ex)
                {
                    Assert.Equal("The logical expression has already been closed.  Only one operator can be used per expression.", ex.Message);
                }
            }
        }
    }
}
