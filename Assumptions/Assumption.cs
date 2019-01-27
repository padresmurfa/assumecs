using System;
using System.Linq;

namespace Assumptions
{
    public class Assumption
    {
        public Assumption Than(object expected, string name = null)
        {
            if (isInvertedAssumption)
            {
                throw new BadGrammar("Not must be specified before a logical operator, not before a terminator");
            }
        
            Expected = expected;

            ExpectedGivenName = name;

            VerifyAssumptionIntegrity();

            Evaluate();

            return this;
        }

        public Assumption To(object expected, string name = null)
        {
            return Than(expected, name);
        }

        public Assumption Is => this;
        
        public Assumption Not
        {
            get
            {
                isInvertedAssumption = true;
                
                return this;
            }
        }

        public Assumption Less
        {
            get
            {
                var inverted = this.CloseExpression(comparable: true);
                
                Expression = (expected, actual) => Check(
                    (((IComparable)actual).CompareTo(expected)) < 0,
                    inverted,
                    () => $"Expected {this.ActualName} ({actual}) to be less than {this.ExpectedName} ({expected})",
                    () => $"Expected {this.ActualName} ({actual}) to not be less than {this.ExpectedName} ({expected})"
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
                    () => $"Expected {this.ActualName} ({actual}) to be greater than {this.ExpectedName} ({expected})",
                    () => $"Expected {this.ActualName} ({actual}) to not be greater than {this.ExpectedName} ({expected})"
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
                    () => $"Expected {this.ActualName} ({actual}) to be less than, or equal to, {this.ExpectedName} ({expected})",
                    () => $"Expected {this.ActualName} ({actual}) to not be less than, or equal to, {this.ExpectedName} ({expected})"
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
                    () => $"Expected {this.ActualName} ({actual}) to be greater than, or equal to, {this.ExpectedName} ({expected})",
                    () => $"Expected {this.ActualName} ({actual}) to not be greater than, or equal to, {this.ExpectedName} ({expected})"
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
                    () => $"Expected {this.ActualName} ({actual}) to be equal to {this.ExpectedName} ({expected})",
                    () => $"Expected {this.ActualName} ({actual}) to not be equal to {this.ExpectedName} ({expected})"
                );

                return this;
            }
        }
        
        public Assumption False()
        {
            var inverted = this.CloseExpression(equatable: true);
            
            Expression = (expected, actual) => Check(
                actual.Equals(expected),
                inverted,
                () => $"Expected {this.ActualName} ({actual}) to be False",
                () => $"Expected {this.ActualName} ({actual}) to not be False"
            );

            this.To(false, "false");

            return this;
        }

        public Assumption True()
        {
            var inverted = this.CloseExpression(equatable: true);
            
            Expression = (expected, actual) => Check(
                actual.Equals(expected),
                inverted,
                () => $"Expected {this.ActualName} ({actual}) to be True",
                () => $"Expected {this.ActualName} ({actual}) to not be True"
            );

            this.To(true, "true");

            return this;
        }
        
        public Assumption InstanceOf(params Type[] types)
        {
            var inverted = this.CloseExpression();
            
            Expression = (expected, actual) => {
                return Check(
                    ((Type[])expected).Any(type => type.IsAssignableFrom((Type)actual)),
                    inverted,
                    () => $"Expected {this.ActualName} ({((Type)actual).FullName}) to be derived from one of the following: {string.Join(", ", ((Type[])expected).Select(t => t.FullName))}",
                    () => $"Expected {this.ActualName} ({((Type)actual).FullName}) to not be derived from any of the following: {string.Join(", ", ((Type[])expected).Select(t => t.FullName))}"
                );
            };

            this.To(types, "types");

            return this;
        }

        public Assumption Null()
        {
            var inverted = this.CloseExpression();
            
            Expression = (expected, actual) => Check(
                actual == null,
                inverted,
                () => $"Expected {this.ActualName} ({actual}) to be null",
                () => $"Expected {this.ActualName} (<null>) to not be null"
            );

            this.To(null, "null");

            return this;
        }

        // INTERNALS
        
        public Assumption(object actual, string actualName, string callerMemberName, string callerSourceFilePath, int callerSourceLineNumber)
        {
            this.Actual = actual;
            this.ActualGivenName = actualName;

            this.CallerMemberName = callerMemberName;
            this.CallerSourceFilePath = callerSourceFilePath;
            this.CallerSourceLineNumber = callerSourceLineNumber;
        }

        private string ExpectedName => this.ExpectedGivenName ?? "expectedValue";
        private string ActualName => this.ActualGivenName ?? "actualValue";

        private string CallerMemberName;
        private string CallerSourceFilePath;
        private int CallerSourceLineNumber;

        private object Actual;
        private string ActualGivenName;

        private object Expected;
        private string ExpectedGivenName;

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
        
        private AssumptionFailure CreateAssumptionFailure(string message = null, Exception innerException = null)
        {
            return new AssumptionFailure(message, innerException, this.CallerMemberName, this.CallerSourceFilePath, this.CallerSourceLineNumber);
        }

        private void VerifyAssumptionIntegrity()
        {
            VerifyAssumptionIntegrityComparability();
            VerifyAssumptionIntegrityEquatability();
        }

        private void VerifyAssumptionIntegrityEquatability()
        {
            if (this.isEquatable)
            {
                if (this.Actual != null && !Equatable(this.Actual))
                {
                    throw CreateAssumptionFailure($"'{this.ActualName}' must be equatable");
                }
                if (this.Expected != null && !Equatable(this.Expected))
                {
                    throw CreateAssumptionFailure($"'{this.ExpectedName}' must be equatable");
                }
            }
        }

        private void VerifyAssumptionIntegrityComparability()
        {
            if (this.isComparable)
            {
                if (this.Actual != null && !Comparable(this.Actual))
                {
                    throw CreateAssumptionFailure($"'{this.ActualName}' must be comparable");
                }
                if (this.Expected != null && !Comparable(this.Expected))
                {
                    throw CreateAssumptionFailure($"'{this.ExpectedName}' must be comparable");
                }
            }
        }

        private bool Comparable(object o)
        {
            return o != null && typeof(IComparable).IsAssignableFrom(o.GetType());
        }

        private bool Equatable(object o)
        {
            return true;
        }

        private void Evaluate()
        {
            var failureReasonFactory = this.Expression(this.Expected, this.Actual);
            if (failureReasonFactory != null)
            {
                var failureReason = failureReasonFactory();
                throw CreateAssumptionFailure(failureReason);
            }
        }
    }
}
