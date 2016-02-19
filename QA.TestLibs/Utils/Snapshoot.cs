namespace QA.TestLibs.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.IO.Compression;
    using System.Xml.Linq;

    public class Snapshoot<T>
    {
        public DateTime Date { get; private set; }
        public Type ItemType { get; private set; }
        public int CompressionRate { get; private set; }

        private int _valueLength;
        private byte[] _value;
        private byte[] _itemSerilalizedValue
        {
            set
            {
                _valueLength = value.Length;
                using (var ms = new MemoryStream())
                {
                    using (var zip = new GZipStream(ms, CompressionLevel.Optimal))
                    {
                        zip.Write(value, 0, value.Length);
                    }
                    _value = ms.ToArray();
                }
            }
        }

        public Snapshoot(T loggedItem)
        {
            ItemType = loggedItem.GetType();
            Date = DateTime.UtcNow;
            MakeSnapshot(loggedItem);
        }

        public void MakeSnapshot(T loggedItem)
        {
            using (var ms = new MemoryStream())
            {
                new BinaryFormatter().Serialize(ms, loggedItem);
                _itemSerilalizedValue = ms.ToArray();
            }
            CompressionRate = (_value.Length * 100) / _valueLength;
        }

        private byte[] GetSerializedValue()
        {
            using (var ms = new MemoryStream(_value))
            {
                using (GZipStream zip = new GZipStream(ms, CompressionMode.Decompress))
                {
                    var o = new byte[_valueLength];
                    zip.Read(o, 0, _valueLength);
                    return o;
                }
            }
        }

        public T GetItem()
        {
            return (T)new BinaryFormatter().Deserialize(new MemoryStream(GetSerializedValue()));
        }
    }
}
