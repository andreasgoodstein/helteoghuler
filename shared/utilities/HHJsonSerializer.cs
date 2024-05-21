using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HelteOgHulerShared.Utilities
{
    public static class HHJsonSerializer
    {
        public static string Serialize<T>(T Obj)
        {
            using (var ms = new MemoryStream())
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
                serializer.WriteObject(ms, Obj);
                byte[] json = ms.ToArray();

                return Encoding.UTF8.GetString(json, 0, json.Length);
            }
        }

        public static T Deserialize<T>(byte[] jsonBytes)
        {
            using (var ms = new MemoryStream(jsonBytes))
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
                var deserializedObj = (T)serializer.ReadObject(ms);

                return deserializedObj;
            }
        }
    }
}