using System;

namespace Assumptions
{
    public class Assumption
    {
        public Assumption Than(object expected, string name = null)
        {
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

        public Assumption Less
        {
            get
            {
                IsComparable = true;

                Expression = (expected, actual) =>
                {
                    var comparison = ((IComparable)expected).CompareTo(actual);

                    if (comparison < 0) return null;

                    return () =>
                    {
                        return $"Expected {this.ActualName} ({actual}) to be less than {this.ExpectedName} ({expected})";
                    };
                };

                return this;
            }
        }

        public Assumption Greater
        {
            get
            {
                IsComparable = true;

                Expression = (expected, actual) =>
                {
                    var comparison = ((IComparable)expected).CompareTo(actual);

                    if (comparison > 0) return null;

                    return () =>
                    {
                        return $"Expected {this.ActualName} ({actual}) to be greater than {this.ExpectedName} ({expected})";
                    };
                };

                return this;
            }
        }

        public Assumption LessOrEqual
        {
            get
            {
                IsComparable = true;

                Expression = (expected, actual) =>
                {
                    var comparison = ((IComparable)expected).CompareTo(actual);

                    if (comparison <= 0) return null;

                    return () =>
                    {
                        return $"Expected {this.ActualName} ({actual}) to be less than or equal to {this.ExpectedName} ({expected})";
                    };
                };

                return this;
            }
        }

        public Assumption GreaterOrEqual
        {
            get
            {
                IsComparable = true;

                Expression = (expected, actual) =>
                {
                    var comparison = ((IComparable)expected).CompareTo(actual);

                    if (comparison >= 0) return null;

                    return () =>
                    {
                        return $"Expected {this.ActualName} ({actual}) to be greater or equal to {this.ExpectedName} ({expected})";
                    };
                };

                return this;
            }
        }

        public Assumption False()
        {
            IsEquatable = true;

            Expression = (expected, actual) =>
            {

                if (actual.Equals(expected)) return null;

                return () =>
                {
                    return $"Expected {this.ActualName} ({actual}) to be false";
                };
            };

            this.To(false, "false");

            return this;
        }

        public Assumption True()
        {
            IsEquatable = true;

            Expression = (expected, actual) =>
            {
                var comparison = ((IComparable)expected).CompareTo(actual);

                if (actual.Equals(expected)) return null;

                return () =>
                {
                    return $"Expected {this.ActualName} ({actual}) to be true";
                };
            };

            this.To(true, "true");

            return this;
        }

        public Assumption Null()
        {
            Expression = (expected, actual) =>
            {

                if (actual == null) return null;

                return () =>
                {
                    return $"Expected {this.ActualName} ({actual}) to be null";
                };
            };

            this.To(null, "null");

            return this;
        }

        public Assumption NotNull()
        {
            Expression = (expected, actual) =>
            {

                if (actual != null) return null;

                return () =>
                {
                    return $"Expected {this.ActualName} ({actual}) to not be null";
                };
            };

            this.To(this, "not null");

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

        private bool isEquatable;
        private bool IsEquatable
        {
            get
            {
                return this.isEquatable;
            }

            set
            {
                this.isEquatable = value;
                VerifyAssumptionIntegrity();
            }
        }

        private bool isComparable;
        private bool IsComparable
        {
            get
            {
                return this.isComparable;
            }

            set
            {
                this.isComparable = value;
                VerifyAssumptionIntegrity();
            }
        }
        private AssumptionFailure CreateAssumptionFailure(string message = null, Exception innerException = null)
        {
            return new AssumptionFailure(message, innerException, this.CallerMemberName, this.CallerSourceFilePath, this.CallerSourceLineNumber);
        }

        private void VerifyAssumptionIntegrity()
        {
            if (this.IsComparable)
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

            if (this.IsEquatable)
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
