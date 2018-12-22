using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sparrow.Web.Common
{
    public static class JsonExtension
    {
        public static string ToJson(this object obj)
        {
            if (obj == null)
            {
                return null;
            }
            return JsonConvert.SerializeObject(obj);
        }

        public static T ToObject<T>(this string jsonString)
        {
            if (string.IsNullOrEmpty(jsonString))
            {
                return default(T);
            }
            return JsonConvert.DeserializeObject<T>(jsonString);
        }
    }
}
