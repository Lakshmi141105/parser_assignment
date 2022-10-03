using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace parser
{
    [StructLayout(LayoutKind.Sequential,Pack = 1)]
    public struct PulseHeader 
    {
       
        public PulseHeader(byte[] data, bool isUnsafe, List<PulseHeader> lst)
        {
                IntPtr dataPtr = IntPtr.Zero;
                try
                {
                    int size = Marshal.SizeOf(typeof(TriggerHeader));
                    dataPtr = Marshal.AllocHGlobal(size);
                    Marshal.Copy(data, 0, dataPtr, size);
                    this = (PulseHeader)Marshal.PtrToStructure(dataPtr, typeof(PulseHeader));
                    lst.Add(this);                
                }
                finally
                {
                    Marshal.FreeHGlobal(dataPtr);
                }
            
        }

        public UInt16 timeOffset;

        public byte width;

        public Int16 PeakValue;

    }
}