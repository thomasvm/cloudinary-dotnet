using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cloudinary
{
    internal class Parameter : IComparable
    {
        private object _value;

        public string Name { get; private set; }

        public string Value
        {
            get
            {
                if (_value is Array)
                    return ConvertArrayToString(_value as Array);
                else
                    return _value.ToString();
            }
        }

        public Parameter(string name, object value)
        {
            Name = name;
            _value = value;
        }

        public override string ToString()
        {
            return string.Format("{0}={1}", Name, Value);
        }

        public int CompareTo(object obj)
        {
            if (!(obj is Parameter))
                return -1;

            return String.CompareOrdinal(Name, ((Parameter)obj).Name);
        }

        private static string ConvertArrayToString(Array a)
        {
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < a.Length; i++)
            {
                if (i > 0)
                    builder.Append(",");

                builder.Append(a.GetValue(i).ToString());
            }

            return builder.ToString();
        }
    }
}
