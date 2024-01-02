using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OpenAI.Playground
{
    public static class StringExtension
    {
        public static string ToJson(this object s)
        {
            if (s == null) return null;
           return JsonSerializer.Serialize(s);
        }

        public static T D<T>(this string json) where T : class
        {
            if (string.IsNullOrWhiteSpace(json)) return null;
           return JsonSerializer.Deserialize<T>(json);
        }
    }
}
