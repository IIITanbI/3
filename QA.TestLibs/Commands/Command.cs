namespace QA.TestLibs.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;

    public class Command
    {
        public List<string> PossibleNames { get; private set; }
        public string Description { get; private set; }
        public CommandManager Manager { get; private set; }
        public MethodInfo Method { get; private set; }

        public ParameterInfo[] ParameterInfos { get; private set; }

        public Command(CommandManager manager, MethodInfo method, CommandAttribute attribute)
        {
            Manager = manager;
            PossibleNames = attribute.Names;
            Description = attribute.Description;
            Method = method;

            ParameterInfos = Method.GetParameters();
        }
    }
}
