using System;
using System.Collections.Generic;

namespace MMPlus.Shared
{
    public static class Extensions
    {
        public static string Get(this IDictionary<string, string> data, string field)
        {
            string stringOutput;
            data.TryGetValue(field, out stringOutput);
            return stringOutput;
        }

        public static T Get<T>(this IDictionary<string, string> data, string field, Func<string, T> convert)
        {
            string stringOutput = Get(data, field);
            T output;
            if (stringOutput != null)
            {
                try
                {
                    output = convert(stringOutput);
                }
                catch
                {
                    output = default(T);
                }
            }
            else
            {
                output = default(T);
            }
            return output;
        }

        public static DateTime GetTimestamp(this IDictionary<string, string> data, string field)
        {
            int utcSeconds = Get(data, field, int.Parse);
            if (utcSeconds == 0)
            {
                return default(DateTime);
            }

            var output = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            output = output.AddSeconds(utcSeconds);

            return output;
        }
    }
}