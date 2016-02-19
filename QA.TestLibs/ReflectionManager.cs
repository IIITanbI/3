namespace QA.TestLibs
{
    using Exceptions;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using XmlDesiarilization;
    using Commands;
    using WpfControls;

    public static class ReflectionManager
    {
        private static Dictionary<Type, XmlType> _type_xmlType = new Dictionary<Type, XmlType>();
        private static Dictionary<XmlType, List<XmlType>> _xmlType_assignableXmlTypes = new Dictionary<XmlType, List<XmlType>>();
        private static List<string> _loadedAssemblies = new List<string>();
        public static List<CommandManager> _managers = new List<CommandManager>();
        public static Dictionary<XmlType, Type> _xmlType_wpfTypeContol = new Dictionary<XmlType, Type>();
        public static Dictionary<Type, string> _wpfTypeContol_type = new Dictionary<Type, string>();

        public static List<XmlType> GetAssignableTypes(Type type)
        {
            var xmlType = GetXmlType(type);
            if (xmlType == null) return new List<XmlType>();

            return _xmlType_assignableXmlTypes[xmlType];
        }
        public static bool IsTypeXml(Type type)
        {
            return _type_xmlType.ContainsKey(type);
        }
        public static XmlType GetXmlType(Type type)
        {
            if (_type_xmlType.ContainsKey(type))
                return _type_xmlType[type];
            return null;
        }


        public static void LoadAssemblies(string pathToLibFolder = null)
        {
            var assemblies = new List<Assembly>();

            assemblies.AddRange(AppDomain.CurrentDomain.GetAssemblies().ToList());

            if (pathToLibFolder == null)
                pathToLibFolder = Directory.GetCurrentDirectory();

            if (Directory.Exists(pathToLibFolder))
            {
                var assemblyFiles = Directory.GetFiles(pathToLibFolder, "*.dll").ToList();
                assemblyFiles.ForEach(af => assemblies.Add(Assembly.LoadFrom(af)));
            }

            var loadedAssemblies = new List<Assembly>();

            foreach (var assembly in assemblies)
            {
                LoadAssembly(assembly);
            }

            if (_wpfTypeContol_type.Values.Count > 0)
            {
                var dict = new Dictionary<Type, Type>();
                foreach (var tmp in _wpfTypeContol_type)
                {
                    dict.Add(tmp.Key, GetXmlTypeByName(tmp.Value).XType);
                }


                foreach (var xmlType in _type_xmlType.Values)
                {
                    var curPair = dict.First(p => p.Value == typeof(XmlBaseType));

                    foreach (var wpfControlTypePair in dict)
                    {
                        if (wpfControlTypePair.Value.IsAssignableFrom(xmlType.XType))
                        {
                            if (curPair.Value.IsAssignableFrom(wpfControlTypePair.Value))
                                curPair = wpfControlTypePair;
                        }
                    }

                    _xmlType_wpfTypeContol.Add(xmlType, curPair.Key);
                }
            }
        }
        public static void LoadAssembly(Assembly assembly)
        {
            var assemblyName = assembly.GetName().Name;
            if (_loadedAssemblies.Contains(assemblyName))
                return;

            var types = assembly.DefinedTypes.ToList();

            foreach (var type in types)
            {
                LoadType(type);
            }

            _loadedAssemblies.Add(assemblyName);
        }

        public static Type GetControlForType(XmlType xmlType)
        {
            return _xmlType_wpfTypeContol[xmlType];
        }

        public static XmlType GetXmlTypeByName(string typeName)
        {
            var type = _type_xmlType.Keys.FirstOrDefault(k => k.Name == typeName);
            if (type == null)
                throw new TestLibsException($"Couldn't find XmlType with name: {typeName}");
            return _type_xmlType[type];
        }

        public static List<XmlType> GetXmlTypes()
        {
            return _xmlType_assignableXmlTypes.Keys.ToList();
        }

        public static void LoadType(Type type)
        {
            if (typeof(XmlBaseType).IsAssignableFrom(type))
            {
                var xmlType = new XmlType(type);
                if (_type_xmlType.ContainsKey(type))
                    throw new TestLibsException($"Found duped xmlType for type: {type}");

                _type_xmlType.Add(type, xmlType);

                _xmlType_assignableXmlTypes.Add(xmlType, new List<XmlType>());

                foreach (var key in _xmlType_assignableXmlTypes.Keys)
                {
                    if (key.XType.IsAssignableFrom(type) && !type.IsAbstract)
                    {
                        _xmlType_assignableXmlTypes[key].Add(xmlType);
                    }
                }

                foreach (var key in _xmlType_assignableXmlTypes.Keys)
                {
                    if (type.IsAssignableFrom(key.XType) && !key.XType.IsAbstract)
                    {
                        _xmlType_assignableXmlTypes[xmlType].Add(key);
                    }
                }
            }
            if (typeof(CommandManagerBase).IsAssignableFrom(type))
            {
                var commandManager = new CommandManager(type);
                _managers.Add(commandManager);
            }
            if (typeof(IWpfTypeControl).IsAssignableFrom(type))
            {
                if (!_wpfTypeContol_type.ContainsKey(type))
                {
                    var typeAtt = type.GetCustomAttribute<WpfTypeControlAttribute>();
                    if (typeAtt != null)
                    {
                        _wpfTypeContol_type.Add(type, typeAtt.XmlTypeName);
                    }
                }
            }
        }

        public static CommandManager GetCommandManagerByTypeName(string managerType)
        {
            var manager = _managers.FirstOrDefault(m => m.CommandManagerType.Name == managerType);
            if (manager == null)
                throw new TestLibsException($"Couldn't find CommandManager with type name: {managerType}");
            return manager;
        }
    }
}
