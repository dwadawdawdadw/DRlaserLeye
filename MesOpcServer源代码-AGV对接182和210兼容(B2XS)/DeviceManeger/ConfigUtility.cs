using System.IO;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;

namespace DeviceManager
{
    public class ConfigUtility
    {
        public static T Load<T>(string filePath) where T : class, new()
        {
            if (File.Exists(filePath) == false)
            {
                var t = new T();
                Save(t, filePath);
            }

            string s = string.Empty;
            using (StreamReader sr = new StreamReader(filePath, Encoding.UTF8))
            {
                s = sr.ReadToEnd();
            }

            return JsonConvert.DeserializeObject<T>(s);
        }

        public static void Save<T>(T t, string filePath) where T : class
        {
            var s = JsonConvert.SerializeObject(t,Formatting.Indented);
            File.WriteAllText(filePath, s, Encoding.UTF8);
        }
        public static void Save<T>(T t, string filePath,bool ignoreNull) where T : class
        {
            if (ignoreNull)
            {
                var setting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
                var s = JsonConvert.SerializeObject(t, Formatting.Indented, setting);
                File.WriteAllText(filePath, s, Encoding.UTF8);
            }
            else
            {
                Save(t, filePath);
            }
        }

        public static void GenerateConfig<T>(T t, string filePath) where T : class
        {
            TextWriter tx = File.CreateText(filePath);
            using (JsonTextWriter js = new JsonTextWriter(tx))
            {
                var type = t.GetType();
                var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                js.WriteStartObject();
                foreach (var pi in properties)
                {
                    js.WritePropertyName(pi.Name);
                    js.WriteValue(0);
                }
                js.WriteEndObject();
            }
        }
    }
}
