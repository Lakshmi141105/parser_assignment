using System;
using System.IO;
using System.Text;

namespace parser {
    public abstract class BaseParserReader
    {
        

        private readonly string _fileName;

        protected BaseParserReader(String fileName) {
            if (!File.Exists(fileName)) {
                throw new FileNotFoundException("File does not exists.", fileName);
            }
            _fileName = fileName;
        }

        public string Name {
            get { return GetType().Name; }
        }

        public DataHeader Parse() {
            using (var br = new BinaryReader(File.Open(_fileName, FileMode.Open, FileAccess.Read, FileShare.Read))) {
              
                return ParseInternal(br);
            }
        }
     

        public void SeekToOffset(BinaryReader br,long offsetToPeSig) {

            br.BaseStream.Seek(offsetToPeSig, SeekOrigin.Begin);
        }

        protected abstract DataHeader ParseInternal(BinaryReader br);
        protected virtual TriggerHeader ParseInternal(BinaryReader br,int offset)
        {
            TriggerHeader th = new TriggerHeader();
            return th;
        }
    }
}