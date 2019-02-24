using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Assumptions.Memory;
using Assumptions.Uncertainty;

namespace Assumptions
{
    public partial class Assumption
    {
        public Assumption Than(object expected, string explanation = null)
        {
            if (isInvertedAssumption)
            {
                throw new BadGrammar("Not must be specified before a logical operator, not before a terminator");
            }

            Expected = expected;

            Explanation = explanation;

            var actual = this.Actual;
                
            if (this.TransformActual != null || this.TransformExpected != null)
            {
                if (this.TransformActual != null)
                {
                    actual = this.TransformActual(this.Actual);
                }

                if (this.TransformExpected != null)
                {
                    expected = this.TransformExpected(this.Expected);
                }
            }
                
            if (this.isEquatable)
            {
                VerifyAssumptionIntegrityEquatability(actual, expected);
            }
                
            if (this.isComparable)
            {
                VerifyAssumptionIntegrityComparability(actual, expected);
            }

            var failureReasonFactory = this.Expression(expected, actual, false);

            var success = failureReasonFactory == null;

            if (Usuality != null)
            {
                var usualityFailedReasonFactory =
                    failureReasonFactory ?? this.Expression(expected, actual, true);
                var isUsuallyTrue = Usuality.Probability.Check(success, usualityFailedReasonFactory, this._sourceCodeLocation);
                if (!success)
                {
                    success = isUsuallyTrue;
                }
            }

            if (!success)
            {
                var failureReason = failureReasonFactory();
                OnAssumptionFailure.Create(failureReason, this.Explanation, null, this._sourceCodeLocation);
                return this;
            }
            
            return this;
        }

        public Assumption To(object expected, string explanation = null)
        {
            return Than(expected, explanation);
        }

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

                Expression = (expected, actual, usualityFailure) => Check(
                    (((IComparable) actual).CompareTo(expected)) < 0,
                    inverted,
                    usualityFailure,
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

                Expression = (expected, actual, usualityFailure) => Check(
                    (((IComparable) actual).CompareTo(expected)) > 0,
                    inverted,
                    usualityFailure,
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

                Expression = (expected, actual, usualityFailure) => Check(
                    (((IComparable) actual).CompareTo(expected)) <= 0,
                    inverted,
                    usualityFailure,
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

                Expression = (expected, actual, usualityFailure) => Check(
                    (((IComparable) actual).CompareTo(expected)) >= 0,
                    inverted,
                    usualityFailure,
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

                Expression = (expected, actual, usualityFailure) => Check(
                    actual.Equals(expected),
                    inverted,
                    usualityFailure,
                    () => $"Expected '{actual}' to be equal to '{expected}'",
                    () => $"Expected '{actual}' to not be equal to '{expected}'"
                );

                return this;
            }
        }

        public Assumption False(string explanation = null)
        {
            var inverted = this.CloseExpression(equatable: true);

            Expression = (expected, actual, usualityFailure) => Check(
                actual.Equals(expected),
                inverted,
                usualityFailure,
                () => $"Expected <true> to be <false>",
                () => $"Expected <false> to not be <false>"
            );

            this.To(false, explanation);

            return this;
        }

        public Assumption True(string explanation = null)
        {
            var inverted = this.CloseExpression(equatable: true);

            Expression = (expected, actual, usualityFailure) => Check(
                actual.Equals(expected),
                inverted,
                usualityFailure,
                () => $"Expected <false> to be <true>",
                () => $"Expected <true> to not be <true>"
            );

            this.To(true, explanation);

            return this;
        }

        public Assumption InstanceOf(Type type, string explanation = null)
        {
            return InstanceOf(new[] {type}, explanation);
        }

        public Assumption InstanceOf(Type[] types, string explanation = null)
        {
            Assume.That(types).Is.Not.Null().And.Is.Not.Empty();

            string InstanceOfHelperTypeNames(object expected)
            {
                var et = (Type[]) expected;

                if (et.Count() == 1)
                {
                    return $"{et[0].FullName}";
                }

                var typeNames = string.Join(", ", et.Select(t => t.FullName));

                return $"one of the following: {typeNames}";
            }

            var inverted = this.CloseExpression();

            Expression = (expected, actual, usualityFailure) =>
            {
                return Check(
                    ((Type[]) expected).Any(type => type.IsAssignableFrom(actual.GetType())),
                    inverted,
                    usualityFailure,
                    () =>
                        $"Expected {((Type) actual.GetType()).FullName} to be derived from {InstanceOfHelperTypeNames(expected)}",
                    () =>
                        $"Expected {((Type) actual.GetType()).FullName} to not be derived from {InstanceOfHelperTypeNames(expected)}"
                );
            };

            this.To(types, explanation);

            return this;
        }

        public Assumption Empty(string explanation = null)
        {
            var inverted = this.CloseExpression();

            Expression = (expected, actual, usualityFailure) =>
            {
                var at = actual?.GetType();
                if (actual == null || typeof(String).IsAssignableFrom(at))
                {
                    return Check(
                        string.IsNullOrEmpty((string) actual),
                        inverted,
                        usualityFailure,
                        () => $"Expected '{actual}' to be empty",
                        () => $"Expected '{actual}' to not be empty"
                    );
                }
                else if (typeof(System.Collections.ICollection).IsAssignableFrom(at))
                {
                    var ac = (System.Collections.ICollection) actual;
                    return Check(
                        ac.Count == 0,
                        inverted,
                        usualityFailure,
                        () => $"Expected collection to be empty",
                        () => $"Expected collection to not be empty"
                    );
                }
                else if (typeof(System.Collections.IEnumerable).IsAssignableFrom(at))
                {
                    var ac = (System.Collections.IEnumerable) actual;
                    var hasAny = ac.GetEnumerator().MoveNext();
                    return Check(
                        !hasAny,
                        inverted,
                        usualityFailure,
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

            Expression = (expected, actual, usualityFailure) => Check(
                actual == null,
                inverted,
                usualityFailure,
                () => $"Expected '{actual}' to be <null>",
                () => $"Expected <null> to not be <null>"
            );

            this.To(null, explanation);

            return this;
        }

        public Assumption Completed(string explanation = null)
        {
            var inverted = this.CloseExpression();

            Expression = (expected, actual, usualityFailure) =>
            {
                Assume.That(actual).Is.InstanceOf(typeof(Action));

                if (inverted)
                {
                    // TODO: handle usualityFailure.  This is a buggy codepath currently.
                    //       Assume.That(()={ ... }).Is.Usually.Not.Completed() is not going to work for not.
                    ((Action) actual)();
                    OnAssumptionFailure.Create("Expected lambda to raise an exception before running to completion",
                        this.Explanation, null, this._sourceCodeLocation);
                    return null;
                }
                else
                {
                    try
                    {
                        ((Action) actual)();
                        if (usualityFailure)
                        {
                            OnAssumptionFailure.Create(
                                "Expected lambda to run to completion without raising an exception", this.Explanation,
                                null, this._sourceCodeLocation);
                            return null;
                        }

                        return null;
                    }
                    catch (Exception ex)
                    {
                        OnAssumptionFailure.Create("Expected lambda to run to completion without raising an exception",
                            this.Explanation, ex, this._sourceCodeLocation);
                        return null;
                    }
                }
            };

            this.To(null, explanation);

            return this;
        }        

        // Certainly is just a synonym for Probably, which is more appropriate to use grammatically if the probability
        // is roughly 100%.
        public Assumption Certainly(
            int minimumSampleSize,
            int withinNumberOfStandardDeviations = 1)
            => Probably(0.99f, minimumSampleSize, withinNumberOfStandardDeviations);

        // CertainlyNot is a synonym for Probably, given a near-zero probability and, joined with inverting the condition.
        public Assumption CertainlyNot(
            int minimumSampleSize,
            int withinNumberOfStandardDeviations = 1)
        {
            return Not.Probably(0.01f, minimumSampleSize, withinNumberOfStandardDeviations);
        }

        public Assumption Probably(
            float probability,
            int minimumSampleSize,
            int withinNumberOfStandardDeviations = 1)
        {
            var f = new OnlineMeanProbabilityFunc
            {
                MinimumSampleSize = minimumSampleSize,
                Probability = probability,
                WithinNumberOfStandardDeviations = withinNumberOfStandardDeviations
            };

            return Usually(f);
        }
        
        public Assumption Consistently(
            int minimumSampleSize,
            int? consistencyDistance = null,
            int withinNumberOfStandardDeviations = 1)
        {
            var f = new ConsistentOverRecentSamplesProbabilityFunc
            {
                MinimumSampleSize = minimumSampleSize,
                ConsistencyDistance = consistencyDistance ?? minimumSampleSize,
                WithinNumberOfStandardDeviations = withinNumberOfStandardDeviations
            };

            return Usually(f);
        }

        public Assumption Usually(ProbabiltyFunc probability)
        {
            // NOTE: cannot place two Usually declarations on the same line!
            if (this.Usuality != null)
            {
                throw new BadGrammar("Usually may only be declared once");
            }

            this.Usuality = UsualityDeclarations.FindOrCreateUsabilityDeclaration(probability, _sourceCodeLocation);

            return this;
        }
    }

    // No-op linguistic sugar
    public partial class Assumption
    {
        public Assumption Does => this;
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

        public Assumption More => Greater;
        public Assumption AtLeast => GreaterThanOrEqual;
    }

    // INTERNALS
    public partial class Assumption
    {
        public Assumption(object actual, SourceCodeLocation sourceCodeLocation)
        {
            this.Actual = actual;

            this._sourceCodeLocation = sourceCodeLocation;
        }

        private SourceCodeLocation _sourceCodeLocation;

        private object Actual;
        private object Expected;

        private Func<object, object> TransformActual;
        private Func<object, object> TransformExpected;
        
        private string Explanation;

        private Func<object, object, bool, Func<string>> Expression;

        private UsualityDeclaration Usuality;
        private bool isInvertedAssumption;        
        private bool isExpressionClosed;
        private bool isEquatable;
        private bool isComparable;
        
        private Func<string> Check(bool test, bool isInverted, bool usualityFailure, Func<string> normal, Func<string> inverted)
        {
            if (isInverted)
            {
                if ((!test) && (!usualityFailure))
                {
                    return null;
                }
                return inverted;
            }
            else
            {
                if ((test) && (!usualityFailure))
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
            }
            
            if (equatable)
            {
                this.isEquatable = true;
            }

            isExpressionClosed = true;
            
            var inverted = this.isInvertedAssumption;
            this.isInvertedAssumption = false;
            return inverted;
        }

        private void VerifyAssumptionIntegrityEquatability(object actual, object expected)
        {
            if (this.isEquatable)
            {
                if (actual != null && !Equatable(actual))
                {
                    OnAssumptionFailure.Create("'actual value' must be equatable", this.Explanation, null, this._sourceCodeLocation);
                    return;
                }
                if (expected != null && !Equatable(expected))
                {
                    OnAssumptionFailure.Create("'expected value' must be equatable", this.Explanation, null, this._sourceCodeLocation);
                    return;
                }
            }
        }

        private void VerifyAssumptionIntegrityComparability(object actual, object expected)
        {
            if (this.isComparable)
            {
                if (actual != null && !Comparable(actual))
                {
                    OnAssumptionFailure.Create("'actual value' must be comparable", this.Explanation, null, this._sourceCodeLocation);
                    return;
                }
                if (expected != null && !Comparable(expected))
                {
                    OnAssumptionFailure.Create("'expected value' must be comparable", this.Explanation, null, this._sourceCodeLocation);
                    return;
                }
            }
        }
        
        private bool Comparable(object o)
        {
            return (o != null) && (o is IComparable);
        }

        private bool Equatable(object o)
        {
            return true;
        }
    }
}
