using System;
using System.Collections.Generic;
using System.IO;

namespace parser
{
    public class Byte4ByteDataReader : BaseParserReader
    {
        public Byte4ByteDataReader(string fileName)
            : base(fileName)
        {
        }

        protected override DataHeader ParseInternal(BinaryReader br)
        {
            var rawHeader = new DataHeader();
            {
                byte[] mk = new byte[6];
                for(int i = 0; i < 6; i++)
                {
                    mk[i] = br.ReadByte();
                }
                rawHeader.magickey = mk;
                rawHeader.numX = br.ReadUInt16();
                rawHeader.numY = br.ReadUInt16();
                rawHeader.numTriggers = br.ReadUInt16();
                rawHeader.numFrames = br.ReadUInt16();
                rawHeader.descLength = br.ReadUInt16();
                char[] desc = new char[rawHeader.descLength];
                for(int d = 0; d < desc.Length; d++)
                {
                    desc[d] = (char)br.ReadByte();
                }
                rawHeader.desc = desc;
                int numPixelsPtrs = rawHeader.numX * rawHeader.numY * rawHeader.numFrames;
                UInt64[] ptrs = new UInt64[numPixelsPtrs];
                for(int ptr = 0;ptr < numPixelsPtrs; ptr++)
                {
                    ptrs[ptr] = br.ReadUInt64();
                }
                rawHeader.pixelDataPtr = ptrs;
               
                Dictionary<int, List<TriggerHeader>> dict = new Dictionary<int, List<TriggerHeader>>();
                
                for(int pixel = 0; pixel < rawHeader.numX * rawHeader.numY; pixel++)
                {
                    SeekToOffset(br,(long)rawHeader.pixelDataPtr[pixel]);
                    //read 50 triggers for each pixel
                    List<TriggerHeader> lstTriggers = new List<TriggerHeader>();
                    int pixelNo = pixel;
                    dict.Add(pixelNo, lstTriggers);
                    for (int trigIndex = 0;trigIndex < rawHeader.numTriggers; trigIndex++)
                    {
                        TriggerHeader th = new TriggerHeader();
                        th.triggerNumber = br.ReadUInt32();
                        th.timeFromLastTrigger = br.ReadUInt16();
                        th.noOfPulses = br.ReadUInt16();
                        //read pulses
                        th.lstPulses = new Dictionary<int,List<PulseHeader>>();
                        List<PulseHeader> pulses = new List<PulseHeader>();
                        th.lstPulses.Add((int)th.triggerNumber, pulses);
                        for (int pulseIndex = 0; pulseIndex < th.noOfPulses; pulseIndex++)
                        {
                            pulses.Add(new PulseHeader()
                            {
                                timeOffset = br.ReadUInt16(),
                                width = br.ReadByte(),
                                PeakValue = br.ReadInt16()
                            });
                        }
                        lstTriggers.Add(th);
                    }
                    rawHeader.triggers = dict;

                }

            }
            return rawHeader;
        }

    }
}