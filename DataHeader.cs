using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace parser
{
    [StructLayout(LayoutKind.Sequential,Pack = 1)]
    public struct DataHeader
    {
        
        public DataHeader(byte[] data, bool isUnsafe)
        {
                
                IntPtr dataPtr = IntPtr.Zero;
                try
                {
                    int size = Marshal.SizeOf(typeof(DataHeader));
                    dataPtr = Marshal.AllocHGlobal(size);
                    Marshal.Copy(data, 0, dataPtr, size);
                    this = (DataHeader)Marshal.PtrToStructure(dataPtr, typeof(DataHeader));
                }
                finally
                {
                    Marshal.FreeHGlobal(dataPtr);
                }
            
        }

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public byte[] magickey;
          
        public UInt16 numX;
    
        public UInt16 numY;
      
        public UInt16 numTriggers;
     
        public UInt16 numFrames;

        public UInt16 descLength;
      
        public char[] desc;
             
        public UInt64[] pixelDataPtr;

        public Dictionary<int,List<TriggerHeader>> triggers;
  
    }
}