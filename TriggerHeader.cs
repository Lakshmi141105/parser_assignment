using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace parser
{
    [StructLayout(LayoutKind.Sequential,Pack = 1)]
    public struct TriggerHeader 
    {
       
        public TriggerHeader(byte[] data, bool isUnsafe, List<TriggerHeader> lst)
        {
                IntPtr dataPtr = IntPtr.Zero;
                try
                {
                    int size = Marshal.SizeOf(typeof(TriggerHeader));
                    dataPtr = Marshal.AllocHGlobal(size);
                    Marshal.Copy(data, 0, dataPtr, size);
                    this = (TriggerHeader)Marshal.PtrToStructure(dataPtr, typeof(TriggerHeader));
                    lst.Add(this);               
                }
                finally
                {
                    Marshal.FreeHGlobal(dataPtr);
                }
            
        }

        //Trigger header
        public UInt32 triggerNumber;

        public UInt16 timeFromLastTrigger;

        public UInt16 noOfPulses;

        public Dictionary<int, List<PulseHeader>> lstPulses;


    }
}