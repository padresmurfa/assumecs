using System;

namespace Assumptions
{
    public class Other : ISuspendOtherThreads
    {
        public Action Suspend()
        {
            return () => { };
        }
    }
}