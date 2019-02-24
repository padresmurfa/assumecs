using System;
using System.Threading;
using System.Diagnostics;


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