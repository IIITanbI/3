namespace QA.TestLibs.ExtensionMethods
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;


    public static class ListExtenshionMethods
    {
        /// <summary>
        /// Give string representation of list
        /// </summary>
        /// <param name="list">List of elements</param>
        /// <param name="startWith">String that will be added to start of each element</param>
        /// <param name="endWith">String that will be added to end of each element (add '\n' to make multiline representation)</param>
        /// <returns>String representation of list</returns>
        public static string ToStringWithList<T>(this List<T> list, string startWith = "\t- ", string endWith = "\n")
        {
            var sb = new StringBuilder();
            foreach (var l in list)
            {
                sb.Append(startWith);
                sb.Append(l.ToString());
                sb.Append(endWith);
            }
            return sb.ToString();
        }

        public static void AddCaseVariantsForFirstChar(this List<string> list)
        {
            var caseNames = new List<string>();

            foreach (var l in list)
            {
                if (l.Length == 0) continue;
                var caseName = (char.IsUpper(l[0]) ? char.ToLower(l[0]) : char.ToUpper(l[0])) + l.Substring(1, l.Length - 1);
                caseNames.Add(caseName);
            }
            list.AddRange(caseNames);
            caseNames.Clear();
            caseNames = list.Distinct().ToList();
            list.Clear();
            list.AddRange(caseNames);
        }
    }
}
