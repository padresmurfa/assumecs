using System;

namespace Assumptions
{
    public class ConsistentOverRecentSamplesProbabilityFunc : ProbabiltyFunc
    {
        // TODO: different distance rules, such as time
        public int ConsistencyDistance;
        
        public int MinimumSampleSize;
        public int WithinNumberOfStandardDeviations = 1;
        
        private OnlineMeanProbabilityFunc ForeRunner;
        private OnlineMeanProbabilityFunc ActualProbabilityFunc;

        public override bool Check(bool success, Func<string> failureReasonFactory, SourceCodeLocation sourceCodeLocation)
        {
            if (ForeRunner == null)
            {
                ForeRunner = new OnlineMeanProbabilityFunc
                {
                    Probability = 0,
                    MinimumSampleSize = MinimumSampleSize,
                    WithinNumberOfStandardDeviations = WithinNumberOfStandardDeviations
                };
                
                ActualProbabilityFunc = new OnlineMeanProbabilityFunc
                {
                    Probability = 0, MinimumSampleSize = ConsistencyDistance + MinimumSampleSize,
                    WithinNumberOfStandardDeviations = WithinNumberOfStandardDeviations
                };
            }

            if (ForeRunner.Count >= MinimumSampleSize)
            {
                ActualProbabilityFunc.Probability = ForeRunner.Mean;

                ActualProbabilityFunc.Check(success, () => null, sourceCodeLocation);
            }

            if (0 == ((int) ForeRunner.Count) % 100)
            {
                var i = 1;
            }

            return ForeRunner.Check(success, failureReasonFactory, sourceCodeLocation);
        }

    }
}