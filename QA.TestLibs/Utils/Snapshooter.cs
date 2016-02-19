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

    public static class Snapshooter
    {
        public static Snapshoot<T> CreateShapshoot<T>(T item)
        {
            return new Snapshoot<T>(item);
        }
    }
}
