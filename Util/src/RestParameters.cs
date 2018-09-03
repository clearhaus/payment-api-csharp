using System;
using System.Reflection;
using System.Collections.Generic;

namespace Clearhaus.Util
{
    // Allows specifying a name to use when converting to url parameter names.
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class ArgName : Attribute
    {
        public string name;

        public ArgName(string name)
        {
            this.name = name;
        }
    }

    // Allow a field/class to be marked optional. Which determines if not supplying
    // it should cause an exception.
    [AttributeUsage(AttributeTargets.Field|AttributeTargets.Class, AllowMultiple = false)]
    public class Optional : Attribute
    {
        public bool optional;

        public Optional()
        {
            optional = true;
        }

        public Optional(bool opt)
        {
            optional = opt;
        }
    }

    // Class used to build rest parameters.
    public class RestParameters
    {
        bool classIsOptional = false;

        internal string GetFieldName(FieldInfo info)
        {
                var res = (ArgName[])info.GetCustomAttributes(typeof(ArgName), false);
                if (res.Length > 0)
                {
                    var attr = res[0];
                    return attr.name;
                }

                return info.Name;
        }

        internal bool GetOptional(FieldInfo info)
        {
                var res = (Optional[])info.GetCustomAttributes(typeof(Optional), false);
                if (res.Length > 0)
                {
                    return res[0].optional;
                }

                return classIsOptional;
        }

        /// <summary>Extract object data as a list, for creating request.</summary>
        public IList<KeyValuePair<string, string>> GetArgs()
        {
            var list = new List<KeyValuePair<string, string>>();

            // Check if class is marked optional
            var classOptAttr = (Optional)this.GetType().GetCustomAttribute(typeof(Optional));
            if (classOptAttr != null)
            {
                classIsOptional = classOptAttr.optional;
            }

            // Iterate through fields, extracting possible argname supplied.
            var fields = this.GetType().GetFields();
            foreach (var info in fields)
            {
                var val = Convert.ChangeType(info.GetValue(this), info.FieldType);

                var key = GetFieldName(info);
                var optional = GetOptional(info);
                var isSet = val is string && !string.IsNullOrWhiteSpace((string)val);

                if (!optional && !isSet)
                {
                    throw new ClrhsException("Field " + info.Name + " must be supplied in class " + this.GetType());
                }

                if (isSet)
                {
                    var pair = new KeyValuePair<string, string>(key, (string)val);
                    list.Add(pair);
                }
            }

            return list;
        }
    }
}
