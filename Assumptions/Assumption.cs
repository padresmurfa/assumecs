using System;
using System.Linq;

namespace Assumptions
{
    public class Assumption
    {
        public Assumption Than(object expected, string explanation = null)
        {
            void Evaluate()
            {
                var failureReasonFactory = this.Expression(this.Expected, this.Actual);
                if (failureReasonFactory != null)
                {
                    var failureReason = failureReasonFactory();
                    throw CreateAssumptionFailure(failureReason);
                }
            }
            
            if (isInvertedAssumption)
            {
                throw new BadGrammar("Not must be specified before a logical operator, not before a terminator");
            }
        
            Expected = expected;

            Explanation = explanation;

            VerifyAssumptionIntegrity();

            Evaluate();

            return this;
        }

        public Assumption To(object expected, string explanation = null)
        {
            return Than(expected, explanation);
        }

        public Assumption Is => this;
        public Assumption Be => this;
        public Assumption An => this;
        public Assumption Of => this;
        public Assumption A => this;
        public Assumption Been => this;
        public Assumption Have => this;
        public Assumption Has => this;
        public Assumption With => this;
        public Assumption Which => this;
        public Assumption The => this;
        public Assumption It => this;

        public Assumption Not
        {
            get
            {
                isInvertedAssumption = true;
                
                return this;
            }
        }
        
        public Assumption And
        {
            get
            {
                if (!this.isExpressionClosed)
                {
                    throw new BadGrammar("Cannot reopen an expression with And before closing it");
                }
                
                this.isExpressionClosed = false;
                
                return this;
            }
        }
        
        public Assumption That(
            object actual)
        {
            this.Actual = actual;
            this.isEquatable = false;
            this.isComparable = false;
            
            return this;
        }

        public Assumption Less
        {
            get
            {
                var inverted = this.CloseExpression(comparable: true);
                
                Expression = (expected, actual) => Check(
                    (((IComparable)actual).CompareTo(expected)) < 0,
                    inverted,
                    () => $"Expected '{actual}' to be less than '{expected}'",
                    () => $"Expected '{actual}' to not be less than '{expected}'"
                );

                return this;
            }
        }

        public Assumption Greater
        {
            get
            {
                var inverted = this.CloseExpression(comparable: true);
                
                Expression = (expected, actual) => Check(
                    (((IComparable)actual).CompareTo(expected)) > 0,
                    inverted,
                    () => $"Expected '{actual}' to be greater than '{expected}'",
                    () => $"Expected '{actual}' to not be greater than '{expected}'"
                );

                return this;
            }
        }

        public Assumption LessThanOrEqual
        {
            get
            {
                var inverted = this.CloseExpression(comparable: true);
                
                Expression = (expected, actual) => Check(
                    (((IComparable)actual).CompareTo(expected)) <= 0,
                    inverted,
                    () => $"Expected '{actual}' to be less than or equal to '{expected}'",
                    () => $"Expected '{actual}' to not be less than or equal to '{expected}'"
                );

                return this;
            }
        }

        public Assumption GreaterThanOrEqual
        {
            get
            {
                var inverted = this.CloseExpression(comparable: true);
                
                Expression = (expected, actual) => Check(
                    (((IComparable)actual).CompareTo(expected)) >= 0,
                    inverted,
                    () => $"Expected '{actual}' to be greater than or equal to '{expected}'",
                    () => $"Expected '{actual}' to not be greater than or equal to '{expected}'"
                );

                return this;
            }
        }

        public Assumption Equal
        {
            get
            {
                var inverted = this.CloseExpression(equatable: true);
                
                Expression = (expected, actual) => Check(
                    actual.Equals(expected),
                    inverted,
                    () => $"Expected '{actual}' to be equal to '{expected}'",
                    () => $"Expected '{actual}' to not be equal to '{expected}'"
                );

                return this;
            }
        }
        
        public Assumption False(string explanation = null)
        {
            var inverted = this.CloseExpression(equatable: true);
            
            Expression = (expected, actual) => Check(
                actual.Equals(expected),
                inverted,
                () => $"Expected <true> to be <false>",
                () => $"Expected <false> to not be <false>"
            );

            this.To(false, explanation);

            return this;
        }

        public Assumption True(string explanation = null)
        {
            var inverted = this.CloseExpression(equatable: true);
            
            Expression = (expected, actual) => Check(
                actual.Equals(expected),
                inverted,
                () => $"Expected <false> to be <true>",
                () => $"Expected <true> to not be <true>"
            );

            this.To(true, explanation);

            return this;
        }
        
        public Assumption InstanceOf(Type type, string explanation = null)
        {
            return InstanceOf(new []{ type }, explanation);
        }
        
        public Assumption InstanceOf(Type[] types, string explanation = null)
        {
            Assume.That(types).Is.Not.Null().And.Is.Not.Empty();
            
            string InstanceOfHelperTypeNames(object expected)
            {
                var et = (Type[])expected;
                
                if (et.Count() == 1)
                {
                    return $"{et[0].FullName}";
                }
                var typeNames = string.Join(", ", et.Select(t => t.FullName));
                
                return $"one of the following: {typeNames}";
            }
            
            var inverted = this.CloseExpression();
            
            Expression = (expected, actual) => {
                return Check(
                    ((Type[])expected).Any(type => type.IsAssignableFrom(actual.GetType())),
                    inverted,
                    () => $"Expected {((Type)actual.GetType()).FullName} to be derived from {InstanceOfHelperTypeNames(expected)}",
                    () => $"Expected {((Type)actual.GetType()).FullName} to not be derived from {InstanceOfHelperTypeNames(expected)}"
                );
            };

            this.To(types, explanation);

            return this;
        }

        public Assumption Empty(string explanation = null)
        {
            var inverted = this.CloseExpression();
            
            Expression = (expected, actual) => {
                var at = actual?.GetType();
                if (actual == null || typeof(String).IsAssignableFrom(at))
                {
                    return Check(
                        string.IsNullOrEmpty((string)actual),
                        inverted,
                        () => $"Expected '{actual}' to be empty",
                        () => $"Expected '{actual}' to not be empty"
                    );
                }
                else if (typeof(System.Collections.ICollection).IsAssignableFrom(at))
                {
                    var ac = (System.Collections.ICollection)actual;
                    return Check(
                        ac.Count == 0,
                        inverted,
                        () => $"Expected collection to be empty",
                        () => $"Expected collection to not be empty"
                    );
                }
                else if (typeof(System.Collections.IEnumerable).IsAssignableFrom(at))
                {
                    var ac = (System.Collections.IEnumerable)actual;
                    var hasAny = ac.GetEnumerator().MoveNext();
                    return Check(
                        !hasAny,
                        inverted,
                        () => $"Expected collection to be empty",
                        () => $"Expected collection to not be empty"
                    );
                }
                throw new BadGrammar($"Empty is not applicable to '{at.FullName}'");
            };
            
            this.To(null, explanation);

            return this;
        }
        
        public Assumption Null(string explanation = null)
        {
            var inverted = this.CloseExpression();
            
            Expression = (expected, actual) => Check(
                actual == null,
                inverted,
                () => $"Expected '{actual}' to be <null>",
                () => $"Expected <null> to not be <null>"
            );

            this.To(null, explanation);

            return this;
        }

        public Assumption Completed(string explanation = null)
        {
            var inverted = this.CloseExpression();
            
            Expression = (expected, actual) =>
            {
                Assume.That(actual).Is.InstanceOf(typeof(Action));

                if (inverted)
                {
                    try
                    {
                        ((Action)actual)();
                        throw CreateAssumptionFailure($"Expected lambda to raise an exception before running to completion");
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
                else
                {
                    try
                    {
                        ((Action)actual)();
                        return null;
                    }
                    catch (Exception ex)
                    {
                        throw CreateAssumptionFailure($"Expected lambda to run to completion without raising an exception", ex);
                    }
                }
            };
            
            this.To(null, explanation);

            return this;
        }


        // INTERNALS
        
        public Assumption(object actual, string callerMemberName, string callerSourceFilePath, int callerSourceLineNumber)
        {
            this.Actual = actual;

            this.CallerMemberName = callerMemberName;
            this.CallerSourceFilePath = callerSourceFilePath;
            this.CallerSourceLineNumber = callerSourceLineNumber;
        }

        private string CallerMemberName;
        private string CallerSourceFilePath;
        private int CallerSourceLineNumber;

        private object Actual;

        private object Expected;
        private string Explanation;

        private Func<object, object, Func<string>> Expression;

        private bool isInvertedAssumption;        
        private bool isExpressionClosed;
        private bool isEquatable;
        private bool isComparable;
        
        private Func<string> Check(bool test, bool isInverted, Func<string> normal, Func<string> inverted)
        {
            if (isInverted)
            {
                if (!test)
                {
                    return null;
                }
                return inverted;
            }
            else
            {
                if (test)
                {
                    return null; 
                }
                return normal;
            }
        }

        private bool CloseExpression(bool comparable = false, bool equatable = false)
        {
            if (this.isExpressionClosed)
            {
                throw new BadGrammar("The logical expression has already been closed.  Only one operator can be used per expression.");
            }
            
            if (comparable)
            {
                this.isComparable = true;
                VerifyAssumptionIntegrity();
            }
            
            if (equatable)
            {
                this.isEquatable = true;
                VerifyAssumptionIntegrity();
            }
            
            isExpressionClosed = true;
            
            var inverted = this.isInvertedAssumption;
            this.isInvertedAssumption = false;
            return inverted;
        }
        
        private AssumptionFailure CreateAssumptionFailure(string message, Exception innerException = null)
        {
            string reason;
            if (string.IsNullOrEmpty(this.Explanation))
            {
                reason = message;
            }
            else if (string.IsNullOrEmpty(message))
            {
                reason = this.Explanation;
            }
            else
            {
                reason = this.Explanation + ". " + message;
            }
            return new AssumptionFailure(reason, innerException, this.CallerMemberName, this.CallerSourceFilePath, this.CallerSourceLineNumber);
        }

        private void VerifyAssumptionIntegrity()
        {
            void VerifyAssumptionIntegrityEquatability()
            {
                if (this.isEquatable)
                {
                    if (this.Actual != null && !Equatable(this.Actual))
                    {
                        throw CreateAssumptionFailure($"'actual value' must be equatable");
                    }
                    if (this.Expected != null && !Equatable(this.Expected))
                    {
                        throw CreateAssumptionFailure($"'expected value' must be equatable");
                    }
                }
            }
    
            void VerifyAssumptionIntegrityComparability()
            {
                if (this.isComparable)
                {
                    if (this.Actual != null && !Comparable(this.Actual))
                    {
                        throw CreateAssumptionFailure($"'actual value' must be comparable");
                    }
                    if (this.Expected != null && !Comparable(this.Expected))
                    {
                        throw CreateAssumptionFailure($"'expected value' must be comparable");
                    }
                }
            }
        
            VerifyAssumptionIntegrityComparability();
            VerifyAssumptionIntegrityEquatability();
        }

        private bool Comparable(object o)
        {
            return o != null && typeof(IComparable).IsAssignableFrom(o.GetType());
        }

        private bool Equatable(object o)
        {
            return true;
        }
    }
}
