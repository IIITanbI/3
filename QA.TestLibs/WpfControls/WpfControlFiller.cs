namespace QA.TestLibs.WpfControls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Controls;
    using XmlDesiarilization;

    public class WpfControlFiller
    {
        private IWpfControlFiller _filler;
        private Lazy<XmlType> _xmlType;

        public WpfControlFiller(IWpfControlFiller filler, WpfControlFillerAttribute att)
        {
            _filler = filler;
            _xmlType = new Lazy<XmlType>(() => ReflectionManager.GetXmlType(att.XmlType));
        }

        public IWpfControlFiller GetFiller()
        {
            return (IWpfControlFiller)Activator.CreateInstance(_filler.GetType());
        }

        public bool IsMatch(Type type)
        {
            return _filler.IsMatch(type);
        }

        public class WCFComparer : IComparer<WpfControlFiller>
        {
            public int Compare(WpfControlFiller x, WpfControlFiller y)
            {
                if (x._filler.IsMatch(y._xmlType.Value.XType))
                    return -1;
                else return 1;
            }
        }
    }
}
