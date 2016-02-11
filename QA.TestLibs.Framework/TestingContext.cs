namespace QA.TestLibs.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using XmlDesiarilization;

    [XmlType("TestingContext config")]
    [XmlLocation("context")]
    public class TestingContext : XmlBaseType, IContext
    {
        public void Add(string path, object value)
        {
            throw new NotImplementedException();
        }

        public void Add(Type type, string name, object value)
        {
            throw new NotImplementedException();
        }

        public bool Contains(Type type, string name)
        {
            throw new NotImplementedException();
        }

        public object ResolveValue(string path)
        {
            throw new NotImplementedException();
        }

        public object ResolveValue(Type type, string name)
        {
            throw new NotImplementedException();
        }
    }
}
