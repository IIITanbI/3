namespace QA.TestLibs.ExtensionMethods
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using Exceptions;

    public static class XmlExtenshionMethods
    {
        /// <summary>
        /// Get element with any of specified names
        /// </summary>
        /// <param name="parentElement"></param>
        /// <param name="names">List of possible names</param>
        /// <returns>Element with any of specified names or null if there is no elements with provided names</returns>
        public static XElement ElementByNames(this XElement parentElement, List<string> names)
        {
            if (names == null) return null;
            foreach (var name in names)
            {
                var el = parentElement.Element(name);
                if (el != null) return el;
            }
            return null;
        }

        /// <summary>
        /// Get attribute with any of specified names
        /// </summary>
        /// <param name="parentElement"></param>
        /// <param name="names">List of possible names</param>
        /// <returns>Attribute with any of specified names or null if there is no attribute with provided names</returns>
        public static XAttribute AttributeByNames(this XElement parentElement, List<string> names)
        {
            if (names == null) return null;
            foreach (var name in names)
            {
                var at = parentElement.Attribute(name);
                if (at != null) return at;
            }
            return null;
        }

        /// <summary>
        /// Get elements with some/any of specified names (all elements for first searched name by default)
        /// </summary>
        /// <param name="names">List of possible names</param>
        /// <param name="searchForAllNames">Default: False. True - to search elements not just with first searhed name</param>
        /// <returns>List of elements with some/any of specified names or empty list if there are no elements</returns>
        public static List<XElement> ElementsByNames(this XElement parentElement, List<string> names, bool searchForAllNames = false)
        {
            var list = new List<XElement>();
            if (names == null) return null;
            foreach (var name in names)
            {
                var els = parentElement.Elements(name).ToList();
                list.AddRange(els);
                if (!searchForAllNames && els.Count > 0) break;
            }
            return list;
        }

        /// <summary>
        /// Get attributes with some/any of specified names (all attributes for first searched name by default)
        /// </summary>
        /// <param name="names">List of possible names</param>
        /// <param name="searchForAllNames">Default: False. True - to search attributes not just with first searhed name</param>
        /// <returns>List of attributes with some/any of specified names or empty list if there are no attributes</returns>
        public static List<XAttribute> AttributesByNames(this XElement parentElement, List<string> names, bool searchForAllNames = false)
        {
            var list = new List<XAttribute>();
            if (names == null) return null;
            foreach (var name in names)
            {
                var atts = parentElement.Attributes(name).ToList();
                list.AddRange(atts);
                if (!searchForAllNames && atts.Count > 0) break;
            }
            return list;
        }

        /// <summary>
        /// Find element and get it value.
        /// </summary>
        /// <param name="namesForChildElement">List of element names</param>
        /// <param name="descriptionForChildElement">Description of element</param>
        /// <returns>Value of element that has name from specified list</returns>
        public static string GetElementValue(this XElement parentElement, List<string> namesForChildElement, string descriptionForChildElement)
        {
            var element = parentElement.ElementByNames(namesForChildElement);
            if (element == null)
                throw new MissedConfigElementException(namesForChildElement, descriptionForChildElement, parentElement);

            return element.Value;
        }

        /// <summary>
        /// Find attribute and get it value.
        /// </summary>
        /// <param name="namesForChildElement">List of attribute names</param>
        /// <param name="descriptionForChildElement">Description of attribute</param>
        /// <returns>Value of attribute that has name from specified list</returns>
        public static string GetAttributeValue(this XElement parentElement, List<string> namesForAttribute, string descriptionForChildAttribute)
        {
            var attribute = parentElement.AttributeByNames(namesForAttribute);
            if (attribute == null)
                throw new MissedConfigElementException(namesForAttribute, descriptionForChildAttribute, parentElement);

            return attribute.Value;
        }

        public static string Value(this XObject element)
        {
            if (element is XElement)
            {
                var el = element as XElement;
                var fn = el.FirstNode;
                if (fn != null && fn.NodeType == System.Xml.XmlNodeType.CDATA)
                    return (el.FirstNode as XCData).Value;
                else
                    return el.Value;
            }

            if (element is XAttribute)
                return (element as XAttribute).Value;

            return null;
        }

        public static XObject ChildByNames(this XElement parentElement, List<string> names)
        {
            if (names == null) return null;
            foreach (var name in names)
            {
                var at = parentElement.Attribute(name);
                if (at != null) return at;
            }

            foreach (var name in names)
            {
                var el = parentElement.Element(name);
                if (el != null) return el;
            }

            return null;
        }
    }
}
