using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;

namespace Assumptions.Memory
{
    public class SuspendOtherThreadsWindowsOs : ISuspendOtherThreads
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr OpenThread(uint dwDesiredAccess, bool bInheritHandle, uint dwThreadId);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern int SuspendThread(IntPtr hThread);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern uint ResumeThread(IntPtr hThread);

        [DllImport("kernel32.dll", SetLastError = true)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool CloseHandle(IntPtr hObject);

        public Action Suspend()
        {
            var currentThread = Thread.CurrentThread.ManagedThreadId;
            var threads = Process.GetCurrentProcess().Threads;

            var resume = new List<IntPtr>();
            for (var l = threads.Count - 1; l >= 0; l--)
            {
                var thread = threads[l];
                if (currentThread == thread.Id)
                {
                    continue;
                }

                var hThread = OpenThread(0x0002, false, (uint) thread.Id);
                if (hThread != null)
                {
                    // TODO: check for state, only suspend if we then will want to resume....
                    if (UInt32.MaxValue != (UInt32)SuspendThread(hThread))
                    {
                        resume.Add(hThread);
                    }
                    else
                    {
                        CloseHandle(hThread);
                    }
                }
            }

            return () =>
            {
                foreach (var hThread in resume)
                {
                    ResumeThread(hThread);
                    CloseHandle(hThread);
                }
            };
        }
    }
}