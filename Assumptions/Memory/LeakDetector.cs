using System;
using System.Runtime.InteropServices;

namespace Assumptions.Memory
{
    public static class LeakDetector
    {
        public static int DefaultThreshold = 88;
        
        public static int DefaultIterations = 1000;

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
        
        

        public static void Detect(Action action, int? threshold = null, int? iterations = null)
        {
            var someIterations = iterations ?? DefaultIterations;
            var someThreshold = threshold ?? DefaultThreshold;
            
            var resume = Suspender.Suspend();
            try
            {
                var before = GarbageCollectAndGetTotalMemory();

                for (var x = 0; x < someIterations; x++)
                {
                    action();
                }
            
                var after = GarbageCollectAndGetTotalMemory();

                var delta = after - before;

                if (delta > someThreshold)
                {
                    throw new MemoryLeakDetected
                    {
                        Iterations = someIterations,
                        Threshold = someThreshold,
                        Before = before,
                        After = after
                    };
                }
            }
            finally
            {
                resume();
            }
        }
    }
}