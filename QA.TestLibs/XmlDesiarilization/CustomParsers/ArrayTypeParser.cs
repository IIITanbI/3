namespace QA.TestLibs.XmlDesiarilization.CustomParsers
{
    using Exceptions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;

    public class ArrayTypeParser : ICustomTypeParser
    {
        public bool IsMatch(Type type)
        {
            return type.IsArray;
        }

        public object Parse(Type type, XObject config, bool isAssignableTypeAllowed, XmlLocation childLocation, IContext context)
        {
            var configEl = config as XElement;
            if (configEl == null)
                throw new TestLibsException($"Following configuration element:\n{config}\n\tcouldn't be cast to type: {type}. XElement expected");

            var childObjType = type.GetElementType();
            var childObjs = XmlParser.Parse(childObjType, configEl, childLocation, isAssignableTypeAllowed, context);

            var memObj = Activator.CreateInstance(type, new object[] { childObjs.Count }) as object[];

            for (int i = 0; i < childObjs.Count; i++)
            {
                memObj[i] = childObjs[i];
            }

            return memObj;
        }
    }
}
