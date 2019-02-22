using System;

namespace Assumptions.Memory
{
    public class Other : ISuspendOtherThreads
    {
        public Action Suspend()
        {
            return () => { };
        }
    }
}