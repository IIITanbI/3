namespace QA.TestLibs.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using XmlDesiarilization;

    [XmlType("TestingContext config")]
    [XmlLocation("context", "testingContext")]
    public class TestContext : XmlBaseType, IContext
    {
        [XmlProperty("List of TestContextItems", IsAssignableTypesAllowed = true, IsRequired = false)]
        [XmlLocation("contextItems", "items")]
        public List<TestContextItem> TestContextItems { get; set; } = new List<TestContextItem>();

        public TestContext ParentContext { get; set; }
        public static Regex BindRegex = new Regex(@"\$\{([\w\s.]+)\}", RegexOptions.Compiled);

        public Dictionary<string, Dictionary<string, object>> ContextValues { get; private set; } = new Dictionary<string, Dictionary<string, object>>();
        public Dictionary<List<string>, object> Managers = new Dictionary<List<string>, object>();

        public void Build()
        {
            var builtContextItems = new List<TestContextItem>();

            foreach (var testContextItem in TestContextItems)
            {
                builtContextItems.AddRange(testContextItem.Build());
            }

            TestContextItems.Clear();
            TestContextItems = builtContextItems;
        }

        public void Init()
        {
            if (ParentContext != null)
            {
                foreach (var itemTypeKey in ParentContext.ContextValues.Keys)
                {
                    ContextValues.Add(itemTypeKey, new Dictionary<string, object>());
                    foreach (var itemNameKey in ParentContext.ContextValues[itemTypeKey].Keys)
                    {
                        ContextValues[itemTypeKey].Add(itemNameKey, ParentContext.ContextValues[itemTypeKey][itemNameKey]);
                    }
                }

                foreach (var managerKey in ParentContext.Managers.Keys)
                {
                    Managers.Add(managerKey, ParentContext.Managers[managerKey]);
                }
            }

            foreach (var contextItem in TestContextItems)
            {
                var typeName = contextItem.TypeName;
                var type = ReflectionManager.GetXmlTypeByName(typeName);

                var configXml = XElement.Parse(ResolveBind(contextItem.ItemConfig.ToString()));

                var obj = XmlParser.Parse(type.XType, configXml, true, null, ContextValues);

                if (objs.Count == 0)
                    throw new TestLibsException($"Empty context item with type: {contextItem.ItemType} or inccorrect inner config:\n{configXml}");

                if (!ContextValues.ContainsKey(typeName))
                    ContextValues.Add(typeName, new Dictionary<string, object>());

                foreach (var obj in objs)
                {
                    var contextObject = obj as ConfigElementBase;
                    if (contextObject == null)
                        throw new TestLibsException($"Error occurs during creating context object: {obj.ToString()}. Couldn't be cast to ConfigElementBase");

                    if (ContextValues[typeName].ContainsKey(contextObject.UniqueKey))
                        throw new TestLibsException($"Error occurs during creating context object: {obj.ToString()}. Object with the same name: {contextObject.UniqueKey} already present");

                    ContextValues[typeName].Add(contextObject.UniqueKey, obj);
                }
            }

            if (ManagerConfigItems != null)
            {
                foreach (var managerItem in ManagerConfigItems)
                {
                    var managerType = ReflectionManager.GetManagerTypeByName(managerItem.Name);
                    var names = ReflectionManager.GetManagerNames(managerType);

                    var managerObj = ConfigManager.ParseStaticFieldsOrCreateWithFields(managerType, managerItem.Content);

                    if (Managers.ContainsKey(names))
                        Managers[names] = managerObj;
                    else
                        Managers.Add(names, managerObj);
                }
            }
        }


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

        public string ResolveBind(string stringWithBind)
        {
            var match = BindRegex.Match(stringWithBind);

            if (!match.Success)
                return stringWithBind;

            var sb = new StringBuilder(stringWithBind);
            var val = System.Security.SecurityElement.Escape(ResolveValue(match.Groups[1].Value).ToString());

            sb.Replace(match.Value, val, match.Index, match.Length);

            return ResolveBind(sb.ToString());
        }
        public object ResolveValue(string name)
        {
            throw new NotImplementedException();
        }

        public object ResolveValue(Type type, string name)
        {
            throw new NotImplementedException();
        }
    }
}
