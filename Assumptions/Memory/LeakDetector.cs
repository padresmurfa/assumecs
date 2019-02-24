using System;
using System.Runtime.InteropServices;

namespace Assumptions.Memory
{
    public static class LeakDetector
    {
        public static RAM DefaultThreshold = RAM.FromBytes(88);
        
        public static int DefaultLeakDetectionIterations = 1000;

        private static ISuspendOtherThreads Suspender {
            get
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    return new SuspendOtherThreadsWindowsOs();
                }
                
                // TODO: OSX, Linux, and ensure that the Windows code actually works.
                
                return new Other();
            }
        }

        private static long GarbageCollectAndGetTotalMemory()
        {
            for (;;)
            {
                var before = GC.GetTotalMemory(true);
                for (var i = 0; i < GC.MaxGeneration; i++)
                {
                    GC.Collect(i, GCCollectionMode.Forced, true, true);
                    GC.WaitForPendingFinalizers();
                }

                var after = GC.GetTotalMemory(true);

                if (after == before)
                {
                    return after;
                }
            }
        }
        

        public static Allocations DetectAllocations(Action action, int? iterations = null)
        {
            var someIterations = iterations ?? 1;
            
            var suspender = (iterations == 1) ? new Other() : Suspender;
            var resume = suspender.Suspend();
            try
            {
                var before = GarbageCollectAndGetTotalMemory();

                for (var x = 0; x < someIterations; x++)
                {
                    action();
                }
            
                var after = GarbageCollectAndGetTotalMemory();

                return new Allocations
                {
                    Iterations = someIterations,
                    Before = before,
                    After = after
                };
            }
            finally
            {
                resume();
            }
        }        

        public static void DetectLeaks(Action action, RAM threshold = null, int? iterations = null)
        {
            var someThreshold = threshold ?? DefaultThreshold;

            var allocations = DetectAllocations(action, iterations);
            
            if (allocations.Delta > someThreshold)
            {
                throw new MemoryLeakDetected
                {
                    Threshold = someThreshold,
                    Allocations = allocations
                };
            }
        }
    }
}