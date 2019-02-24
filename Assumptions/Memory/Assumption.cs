using System;
using Assumptions.Memory;

namespace Assumptions
{
    public partial class Assumption
    {
        public Assumption Allocates
        {
            get
            {
                TransformActual = (actual) =>
                {
                    var d = (Action) this.Actual;
                    var allocations = LeakDetector.DetectAllocations(d);
                    return allocations.Delta.ToInt();
                };

                TransformExpected = (expected) =>
                {
                    if (expected is RAM)
                    {
                       return ((RAM)expected).ToInt();
                    }

                    return expected;
                };
                    
                return this;
            }
        }

        public Assumption Leaks(string explanation = null, int? threshold = null, int? iterations = null) => Leak(explanation, threshold, iterations);

        public Assumption Leak(string explanation = null, int? threshold = null, int? iterations = null)
        {
            var inverted = this.CloseExpression();

            Expression = (expected, actual, usualityFailure) =>
            {
                Assume.That(actual).Is.InstanceOf(typeof(Action));

                if (inverted)
                {
                    // i.e. the normal case, we expect not to leak memory
                    try
                    {
                        Memory.LeakDetector.DetectLeaks((Action)actual, threshold, iterations);
                    }
                    catch (Memory.MemoryLeakDetected e)
                    {
                        OnAssumptionFailure.Create("Did not expect lambda to leak memory",
                            this.Explanation, e, this._sourceCodeLocation);
                    }
                }
                else
                {
                    // i.e. we expect to actually leak memory
                    try
                    {
                        Memory.LeakDetector.DetectLeaks((Action)actual, threshold, iterations);
                        
                        OnAssumptionFailure.Create(
                            "Expected lambda to leak memory", this.Explanation,
                            null, this._sourceCodeLocation);
                    }
                    catch (Memory.MemoryLeakDetected)
                    {
                    }
                }
                return null;
            };

            this.To(null, explanation);

            return this;
        }
    }
}
