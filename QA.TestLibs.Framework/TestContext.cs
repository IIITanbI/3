namespace QA.TestLibs.Framework
{
    using Exceptions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using XmlDesiarilization;
    using Commands;
    using System.Reflection;

    [XmlType("TestingContext config")]
    [XmlLocation("context", "testingContext")]
    public class TestContext : XmlBaseType, IContext
    {
        private static Lazy<MethodInfo> _initMethod = new Lazy<MethodInfo>(() => (typeof(XmlBaseType).GetMethod("Init")));

        [XmlProperty("List of TestContextItems", IsAssignableTypesAllowed = true, IsRequired = false)]
        [XmlLocation("contextItems")]
        public List<TestContextItem> TestContextItems { get; set; } = new List<TestContextItem>();

        [XmlProperty("List of Managers", IsRequired = false)]
        [XmlLocation("managerItems", "managers")]
        public List<CommandManagerItem> CommandManagersItems { get; set; } = new List<CommandManagerItem>();

        public TestContext ParentContext { get; set; }
        public static Regex BindRegex = new Regex(@"\$\{([\w\s.]+)\}", RegexOptions.Compiled);

        public Dictionary<string, Dictionary<string, object>> ContextValues { get; set; } = new Dictionary<string, Dictionary<string, object>>();
        public Dictionary<string, Dictionary<string, object>> Managers = new Dictionary<string, Dictionary<string, object>>();

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

        public void Initialize()
        {
            ContextValues.ToList().ForEach(cv => cv.Value.Clear());
            ContextValues.Clear();

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

                if (Managers.Count == 0)
                {
                    foreach (var managerKey in ParentContext.Managers.Keys)
                    {
                        Managers.Add(managerKey, ParentContext.Managers[managerKey]);
                    }
                }
            }

            foreach (var contextItem in TestContextItems)
            {
                var typeName = contextItem.TypeName;
                var type = ReflectionManager.GetXmlTypeByName(typeName);

                var configXml = XElement.Parse(ResolveBind(contextItem.ItemConfig.ToString()));

                var obj = XmlParser.Parse(type.XType, configXml, false, null, this);
                _initMethod.Value.Invoke(obj, null);

                if (!ContextValues.ContainsKey(typeName))
                    ContextValues.Add(typeName, new Dictionary<string, object>());

                var contextObject = obj as XmlBaseType;
                if (contextObject == null)
                    throw new TestLibsException($"Error occurs during creating context object: {obj.ToString()}. Couldn't be cast to ConfigElementBase");

                if (ContextValues[typeName].ContainsKey(contextObject.UniqueName))
                    throw new TestLibsException($"Error occurs during creating context object: {obj.ToString()}. Object with the same name: {contextObject.UniqueName} already present");

                ContextValues[typeName].Add(contextObject.UniqueName, obj);
            }

            if (Managers.Count == 0)
            {
                foreach (var managerItem in CommandManagersItems)
                {
                    var manager = ReflectionManager.GetCommandManagerByTypeName(managerItem.ManagerType);

                    object managerConfig = null;
                    if (manager.ConfigType != null)
                    {
                        managerConfig = XmlParser.Parse(manager.ConfigType, managerItem.Config.Elements().First(), true, null, this);
                        _initMethod.Value.Invoke(managerConfig, null);
                    }

                    var managerObj = manager.CreateObject(managerConfig);

                    if (!Managers.ContainsKey(managerItem.ManagerType))
                        Managers.Add(managerItem.ManagerType, new Dictionary<string, object>());

                    if (Managers[managerItem.ManagerType].ContainsKey(managerItem.Name ?? managerItem.ManagerType))
                        Managers[managerItem.ManagerType][managerItem.Name ?? managerItem.ManagerType] = managerObj;
                    else
                        Managers[managerItem.ManagerType].Add(managerItem.Name ?? managerItem.ManagerType, managerObj);
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
            if (!ContextValues.ContainsKey(type.Name))
                return false;
            if (!ContextValues[type.Name].ContainsKey(name))
                return false;
            return true;
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
            return ContextValues[type.Name][name];
        }
    }
}
