using System;

namespace Assumptions.Uncertainty
{
    public class OnlineMeanProbabilityFunc : ProbabiltyFunc
    {
        public float Probability;
        public int MinimumSampleSize;
        public int WithinNumberOfStandardDeviations = 1;
        
        public float Count;
        public float Mean;
        private float m2;
        
        private const int cycleAtCount = int.MaxValue >> 1;

        private float? Variance
        {
            get
            {
                // the algorithm is not designed to work until at least 2 samples have been
                // acquires
                if (Count < 2) return null;
                
                return m2 / Count;
            }
        }

        private float? StableVariance
        {
            get
            {
                if (Count < MinimumSampleSize)
                {
                    return null;
                }

                return Variance;
            }
        }

        private float LowerBound => Mean - (StableVariance.Value * (float) WithinNumberOfStandardDeviations);
        private float UpperBound => Mean + (StableVariance.Value * (float) WithinNumberOfStandardDeviations);
        
        public override bool Check(bool success, Func<string> failureReasonFactory, SourceCodeLocation sourceCodeLocation)
        {
            var newValue = success ? 1.0f : 0.0f;
            
            // using Welford's algorithm for calculating an online mean/variance
            // see: https://en.wikipedia.org/wiki/Algorithms_for_calculating_variance
            void update()
            {
                Count++;
                var delta = newValue - Mean;
                Mean += delta / Count;
                var delta2 = newValue - Mean;
                m2 += delta * delta2;

                if (Count > cycleAtCount)
                {
                    Count /= 2.0f;
                    m2 /= 2.0f;
                }
            }
            update();

            void reset()
            {
                Count = Mean = m2 = 0;
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
                        $"Accumulated success probability violation. actual={Mean}±{variance} ({currentLower}..{currentUpper}) after {(int) Count} iterations.  expected={Probability}, within {WithinNumberOfStandardDeviations} stddevs";
                    reset();
                    OnAssumptionFailure.Create(msg, failureReasonFactory(), null, sourceCodeLocation);
                    return false;
                }
            }

            return true;
        }
    }
}