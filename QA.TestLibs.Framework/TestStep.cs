namespace QA.TestLibs.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using XmlDesiarilization;
    using TestLogger;
    using Exceptions;
    using System.Text.RegularExpressions;

    [XmlType("Test step config")]
    [XmlLocation("step", "testingStep")]
    public class TestStep : XmlBaseType
    {
        [XmlProperty("Name of TestStep")]
        public string Name { get; set; }

        [XmlProperty("Description for TestStep")]
        public string Description { get; set; }

        [XmlProperty("Order of TestStep. Pre, Case, CasePost or Post", IsRequired = false)]
        [XmlLocation(XmlLocationType.Attribute | XmlLocationType.Element, "order")]
        public Order StepOrder { get; set; } = Order.Case;

        [XmlProperty("Is step skipped on fail", IsRequired = false)]
        [XmlLocation(XmlLocationType.Attribute | XmlLocationType.Element, "skipOnFail")]
        public bool IsSkippedOnFail { get; set; } = false;

        [XmlProperty("Number of tries for the step execution", IsRequired = false)]
        [XmlLocation(XmlLocationType.Attribute | XmlLocationType.Element, "retries")]
        public int TryCount { get; set; } = 1;

        [XmlProperty("TestStep phrase")]
        [XmlLocation(XmlLocationType.Element | XmlLocationType.Attribute | XmlLocationType.Value)]
        public string Phrase { get; set; }

        public string Manager { get; private set; }
        public string CommandName { get; private set; }
        public List<string> Parameters { get; private set; } = new List<string>();

        public void Execute(TestContext context, TestLogger log)
        {
            log.INFO($"Start executing of TestStep: {Name}");
            log.INFO($"Description: {Description}");

            var managerParts = Manager.Split('.');
            var managerTypeName = managerParts[0];
            string managerName = null;
            if (managerParts.Length == 2)
                managerName = managerParts[1];
            else if (managerParts.Length > 2)
                throw new TestLibsException($"Unexpected Manager: {Manager}");

            var manager = ReflectionManager.GetCommandManagerByTypeName(managerTypeName);
            var command = manager.Commands.First(c => c.PossibleNames.Contains(CommandName));
            var parameterInfos = command.ParameterInfos;

            var managerObj = context.Managers[managerTypeName][managerName ?? managerTypeName];
            var curParamStrIndex = 0;
            var parObjs = new List<object>();

            for (int i = 0; i < parameterInfos.Length; i++)
            {
                var pInfo = parameterInfos[i];

                if (pInfo.ParameterType == typeof(ILogger))
                {
                    parObjs.Add(log);
                    continue;
                }
                if (pInfo.ParameterType == typeof(IContext))
                {
                    parObjs.Add(context);
                    continue;
                }

                var parStr = Parameters[curParamStrIndex];
                curParamStrIndex++;

                if (parStr.StartsWith("$"))
                {
                    var objStr = context.ResolveBind(parStr.Substring(2, parStr.Length - 3));
                    var parObj = context.ResolveValue(objStr);
                    if (pInfo.ParameterType.IsAssignableFrom(parObj.GetType()))
                    {
                        parObjs.Add(parObj);
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
                else
                {
                    if (pInfo.ParameterType == typeof(string))
                    {
                        parObjs.Add(context.ResolveBind(parStr));
                    }
                    else
                    {
                        log.TRACE("Parameter type is Not string.");
                        throw new TestLibsException($"Couldn't put object with type: String to parameter with name: {pInfo.Name} and type: {pInfo.ParameterType.Name} to call method: {CommandName} from manager: {Manager}.");
                    }
                }
            }

            var result = command.Method.Invoke(managerObj, parObjs.ToArray());
        }

        public static Regex KeyWordRegex = new Regex(@"((?:\$?\{.*?\})(?:[^{]*\})*)", RegexOptions.Compiled);
        private string TrimBraces(string value)
        {
            if (value.StartsWith("$"))
                return value;
            else return value.Substring(1, value.Length - 2);
        }
        public bool IsBuilded { get; set; } = false;
        public void Build()
        {
            if (IsBuilded) return;

            var matches = KeyWordRegex.Matches(Phrase);
            if (matches.Count == 0)
                throw new TestLibsException($"Couldn't parse test step from phrase: {Phrase}. There are no required marks");

            if (matches.Count < 2)
                throw new TestLibsException($"Couldn't parse test step from phrase: {Phrase}. There are not enough required marks");

            Manager = TrimBraces(matches[0].Value);
            CommandName = TrimBraces(matches[1].Value);

            for (int i = 2; i < matches.Count; i++)
            {
                Parameters.Add(TrimBraces(matches[i].Value));
            }

            IsBuilded = true;
        }

        public enum Order
        {
            Pre, Case, CasePost, Post
        }
    }
}
