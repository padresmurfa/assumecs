using System;

namespace Assumptions.Memory
{
    public class Allocations
    {
        public int Iterations;
        
        public RAM Before;
            
        public RAM After;
            
        public RAM Delta => After - Before;
    }

    public class MemoryLeakDetected : Exception
    {
        public RAM Threshold;

        public Allocations Allocations;
    }
}