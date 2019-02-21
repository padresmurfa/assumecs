using System;

namespace Assumptions
{
    public interface ISuspendOtherThreads
    {
        Action Suspend();
    }
}