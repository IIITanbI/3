namespace QA.TestLibs.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Reflection;

    public class CommandManager
    {
        public List<Command> Commands { get; private set; } = new List<Command>();
        public List<string> PossibleNames { get; private set; }
        public string Description { get; private set; }
        public Type CommandManagerType { get; private set; }

        public Type ConfigType { get; set; }

        public CommandManager(Type type)
        {
            var attribute = type.GetCustomAttribute<CommandManagerAttribute>();
            PossibleNames = attribute.Names;
            Description = attribute.Description;
            CommandManagerType = type;
            ConfigType = attribute.ConfigType;
            Init();
        }

        public void Init()
        {
            var methods = CommandManagerType.GetMethods();

            foreach (var method in methods)
            {
                var commandAtt = method.GetCustomAttribute<CommandAttribute>(true);
                if (commandAtt == null) continue;
                var cmd = new Command(this, method, commandAtt);
                Commands.Add(cmd);
            }
        }

        public object CreateObject(object managerConfig)
        {
            return Activator.CreateInstance(CommandManagerType, new[] { managerConfig });
        }
    }
}
