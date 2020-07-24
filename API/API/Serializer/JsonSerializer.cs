using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Serializer
{
    public sealed class ApiJsonSerializer
    {
        private static readonly JsonSerializerSettings _settings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Ignore
        };

        public static string SerializeObject(object sObject)
        {
            return JsonConvert.SerializeObject(sObject, Formatting.Indented, _settings);
        }
    }
}
