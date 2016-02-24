namespace QA.TestLibs.ExtensionMethods
{
    using Exceptions;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;


    public static class ObjectExtensionMethods
    {
        public static List<object> GetChilds(this object genericOrArrayObject)
        {
            var type = genericOrArrayObject.GetType();

            var childObjs = new List<object>();

            if (type.IsArray || type.IsGenericType)
            {
                if (type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
                    foreach (var child in ((IDictionary)genericOrArrayObject).Values)
                        childObjs.Add(child);
                else
                    foreach (var child in (IEnumerable)genericOrArrayObject)
                        childObjs.Add(child);

                return childObjs;
            }

            throw new TestLibsException($"Couldn't get children from type: {type}");
        }
    }
}
