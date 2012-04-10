using System;
using System.IO;
using System.Text;

namespace Cloudinary.Parameters
{
    internal class Parameter : IComparable
    {
        protected internal const string BOUNDARY = "SoMeTeXtWeWiLlNeVeRsEe";
        protected const string LINE = "\r\n";

        private object _value;

        public string Name { get; private set; }

        public string Value
        {
            get
            {
                if (_value is Array)
                    return ConvertArrayToString(_value as Array);

                return _value.ToString();
            }
        }

        protected Parameter(string name)
        {
            Name = name;
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

        public virtual void WriteTo(StreamWriter writer)
        {
            writer.Write("--" + BOUNDARY + LINE);
            writer.Write("Content-Disposition: form-data; name=\"" + Name + "\"" + LINE + LINE + Value + LINE);
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
