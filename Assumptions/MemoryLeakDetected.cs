using System;

namespace Assumptions
{
    public class MemoryLeakDetected : Exception
    {
        public int Iterations;
            
        public long Threshold;
            
        public long Before;
            
        public long After;
            
        public long Delta
        {
            get => After - Before;
        }
    }
}