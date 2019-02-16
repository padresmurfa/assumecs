using System;
using Assumptions;

namespace Assumptions
{
    public class AccumulatedProbabilityFunc : ProbabiltyFunc
    {
        public float Probability;
        public int MinimumSampleSize;
        public int WithinNumberOfStandardDeviations = 1;
        
        private float count;
        private float mean;
        private float m2;

        private float? Variance
        {
            get
            {
                // the algorithm is not designed to work until at least 2 samples have been
                // acquires
                if (count < 2) return null;
                
                return m2 / count;
            }
        }

        private float? StableVariance
        {
            get
            {
                if (count < MinimumSampleSize)
                {
                    return null;
                }

                return Variance;
            }
        }

        private float LowerBound => mean - (StableVariance.Value * (float) WithinNumberOfStandardDeviations);
        private float UpperBound => mean + (StableVariance.Value * (float) WithinNumberOfStandardDeviations);
        
        public override bool Check(bool success, Func<string> failureReasonFactory, SourceCodeLocation sourceCodeLocation)
        {
            var newValue = success ? 1.0f : 0.0f;
            
            // using Welford's algorithm for calculating an online mean/variance
            // see: https://en.wikipedia.org/wiki/Algorithms_for_calculating_variance
            void update()
            {
                count++;
                var delta = newValue - mean;
                mean += delta / count;
                var delta2 = newValue - mean;
                m2 += delta * delta2;
            }
            update();

            void reset()
            {
                count = mean = m2 = 0;
            }

            // only perform the check if our variance is stable enough
            var variance = StableVariance;
            if (variance.HasValue)
            {
                var currentUpper = UpperBound;

                var okUpper = currentUpper > Probability;
                if (!okUpper)
                {
                    var currentLower = LowerBound;
                    var msg =
                        $"Accumulated success probability violation. actual={mean}±{variance} ({currentLower}..{currentUpper}) after {(int) count} iterations.  expected={Probability}, within {WithinNumberOfStandardDeviations} stddevs";
                    reset();
                    OnAssumptionFailure.Create(msg, failureReasonFactory(), null, sourceCodeLocation);
                    return false;
                }
            }

            return true;
        }
    }
}