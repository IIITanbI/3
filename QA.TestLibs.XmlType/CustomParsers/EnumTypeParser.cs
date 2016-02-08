namespace QA.TestLibs.XmlType.CustomParsers
{
    using Exceptions;
    using ExtensionMethods;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;

    public class EnumTypeParser : ICustomTypeParser
    {
        public bool IsMatch(Type type)
        {
            return type.IsEnum;
        }

        public object Parse(Type type, XObject config, bool isAssignableTypeAllowed, XmlLocation childLocation, IContext context)
        {
            var value = config.Value();

            try
            {
                var enumVal = Enum.Parse(type, value);
                if (!Enum.IsDefined(type, enumVal))
                    throw new TypeCastException(value, type, config);
                return enumVal;
            }
            catch (TypeCastException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new TypeCastException(value, type, config, ex);
            }
        }
    }
}
