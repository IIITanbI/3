namespace QA.TestLibs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    
    public class AllowedNamesAttribute : Attribute
    {
        public List<string> AllowedNames { get; protected set; }
    }
}
