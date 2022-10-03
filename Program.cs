using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace parser {
    internal class Program {
        private static readonly string FileName = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.FullName, "assignment_image.bin");
       
        private static void Main(string[] args) {
          
            Byte4ByteDataReader readers = new Byte4ByteDataReader(FileName);            
            Console.WriteLine(readers.Name);
            DataHeader dataHeader = readers.Parse();
                     
            List<List<TriggerHeader>> triggers = dataHeader.triggers.Values.ToList();
            List<TriggerHeader> flattenedLstTriggers = triggers.SelectMany(x => x).ToList();           

            TriggerHeader res = flattenedLstTriggers.Where(p => p.triggerNumber == 138320).FirstOrDefault();

            List<PulseHeader> triggerPulses = new List<PulseHeader>();

            triggerPulses = res.lstPulses.Values.ToList().SelectMany(l=>l).ToList();

            List<ushort> timeOffsets = new List<ushort>();

            //1. What is the Time Offset at trigger number 138320?
            foreach(PulseHeader ph in triggerPulses)
            {
                timeOffsets.Add(ph.timeOffset);
            }

            //get all pulses flat list
            List<PulseHeader> pulses =  flattenedLstTriggers.Select(i => i.lstPulses.Values).ToList().SelectMany(o => o).ToList().SelectMany(k => k).ToList();

            //2. what is the most common Time Offset?

            ushort mostCommonTimeOffset = pulses.GroupBy(i => i).OrderByDescending(grp => grp.Count()).Select(grp => grp.Key).First().timeOffset;

            Console.WriteLine(string.Format(
                   " - Magic Key: {0}\n - numX: {1}\n - numY: {2}\n - numTriggers: {3}\n - numFrames: {4}\n - descLength: {5}\n" +
                   " - Desc: {6}\n ",
               
                   System.Text.Encoding.Default.GetString(dataHeader.magickey),
                   dataHeader.numX,
                   dataHeader.numY,
                   dataHeader.numTriggers,
                   dataHeader.numFrames,
                   dataHeader.descLength,
                   new String(dataHeader.desc)
                   ));

            Console.WriteLine("---------------------RESULT------------------------\n");

            Console.WriteLine("---------------------Time Offsets at Trigger no: 138320------------------\n");

            for(int to =0;to<timeOffsets.Count;to++)
            {
                Console.WriteLine(" - Time Offset {0} - {1}\n",to+1,timeOffsets[to]);
            }

            Console.WriteLine("---------------------Most Common Time Offsets in all Pulses------------------\n");

            Console.WriteLine(" - Most commong Time Offset - {0} \n", mostCommonTimeOffset);

            Console.WriteLine("---------------------END OF RESULT------------------------");

            Console.ReadKey();
        }
    }
}