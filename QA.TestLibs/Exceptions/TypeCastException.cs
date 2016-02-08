namespace QA.TestLibs.Exceptions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using ExtensionMethods;

    public class TypeCastException : Exception
    {
        public static string ExtractAllowedValues(Type type)
        {
            if (type.IsEnum)
            {
                var values = Enum.GetNames(type).ToList();
                return $"Enum with type: {type}. Possible values:\n{values.ToStringWithList()}";
            }

            return $"Type: {type}";
        }

        public TypeCastException(string value, Type destType, XObject config, Exception innerException = null)
            : base($"Couldnt convert value: {value} from config:\n\t{config}\n\tto {ExtractAllowedValues(destType)}", innerException)
        {

        }
    }
}
