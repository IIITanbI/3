namespace QA.TestLibs.ExtensionMethods
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Exceptions;

    public static class StringExtenshionMethods
    {
        /// <summary>
        /// Cast string to value of specified enum.
        /// </summary>
        /// <typeparam name="T">Enum type</typeparam>
        /// <param name="enumStringValue">String value to cast to enum value</param>
        /// <returns>Enum value</returns>
        public static T ToEnum<T>(this string enumStringValue)
            where T : struct
        {
            var t = typeof(T);

            if (!t.IsEnum)
                throw new TestLibsException($"Couldn't convert string: {enumStringValue} to enum: {t.Name} because it's not an enum type");

            T enumVal;
            var r = Enum.TryParse(enumStringValue, out enumVal);
            if (!r)
                throw new TestLibsException($"Couldn't convert string: {enumStringValue} to enum {t.Name}. Allowed values are:\n{Enum.GetValues(t).Cast<T>().ToList().ToStringWithList()}");

            return enumVal;
        }

        public static object ToType(this string stringToCast, Type destType)
        {
            if (destType.Name == "String")
                return stringToCast;

            if (destType.IsEnum)
            {
                var itype = typeof(StringExtenshionMethods);
                var m = itype.GetMethod("ToEnum");
                var im = m.MakeGenericMethod(new[] { destType });
                return im.Invoke(null, new[] { stringToCast });
            }
            
            if (destType.IsPrimitive || destType.Name == "DateTime")
            {
                var m = destType.GetMethod("TryParse", new Type[] { typeof(string), destType.MakeByRefType() });
                object[] args = { stringToCast, null };
                var r = (bool)m.Invoke(null, args);

                if (!r)
                    throw new TestLibsException($"Couldn't convert string: {stringToCast} to primitive type {destType.Name}.");

                return args[1];
            }

            throw new TestLibsException($"Unknow converting. Couldn't convert string: {stringToCast} to type {destType.Name}.");
        }
    }
}
