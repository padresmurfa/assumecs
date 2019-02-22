using System;

namespace Assumptions.Memory
{
    public interface ISuspendOtherThreads
    {
        Action Suspend();
    }
}