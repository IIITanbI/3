namespace QA.TestLibs
{
    using System;

    public interface IContext
    {
        object ResolveValue(string path);
        object ResolveValue(Type type, string name);

        bool Contains(Type type, string name);

        void Add(Type type, string name, object value);
        void Add(string path, object value);
    }
}
