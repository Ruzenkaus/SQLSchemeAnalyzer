using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace hz.Classes
{
    public class SQLSchemeAnalyzer
    {

        private string? Scheme;
        public string? _Scheme { get; set; }
        public SQLSchemeAnalyzer(string scheme)
        {
            Scheme = scheme;
        }

        public SQLSchemeAnalyzer()
        {
            Scheme = null;
        }

        
            public Tuple<Dictionary<string, string>, Dictionary<string, string>> SplittingScheme(string sch)
            {
                var outerProperties = new Dictionary<string, string>();
                var innerProperties = new Dictionary<string, string>();

                using (var reader = new JsonTextReader(new StringReader(sch)))
                {
                    var jObject = JObject.Load(reader);

                    if (jObject["properties"] is JObject properties)
                    {
                        foreach (var property in properties)
                        {
                            var propertyName = property.Key;
                            var propertyDetails = property.Value as JObject;

                            if (propertyDetails != null && propertyDetails["type"] != null)
                            {
                                var propertyType = propertyDetails["type"].ToString();
                                outerProperties[propertyName] = propertyType;

                                if (propertyDetails["properties"] is JObject innerProps)
                                {
                                    foreach (var innerProp in innerProps)
                                    {
                                        var innerPropName = innerProp.Key;
                                        var innerPropDetails = innerProp.Value as JObject;

                                        if (innerPropDetails != null && innerPropDetails["type"] != null)
                                        {
                                            var innerPropType = innerPropDetails["type"].ToString();
                                            innerProperties[$"{propertyName}.{innerPropName}"] = innerPropType;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                return new Tuple<Dictionary<string, string>, Dictionary<string, string>>(outerProperties, innerProperties);
            }
        
    }
}
