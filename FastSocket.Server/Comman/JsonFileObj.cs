using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace FastSocket.Server.Comman
{
    public static class JsonFileObj
    {
        public static T GetJsonObjFromJsonFile<T>(string filePath) where T : class
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                return default(T);
            }
            else
            {
                if (File.Exists(filePath))
                {
                    StreamReader fileStream = new StreamReader(filePath);
                    string jsonStr = fileStream.ReadToEnd();
                    fileStream.Close();
                    fileStream = null;
                    if (string.IsNullOrWhiteSpace(jsonStr))
                    {
                        return default(T);
                    }
                    else
                    {
                        DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(T));
                        var jsonStrStream = new MemoryStream(Encoding.UTF8.GetBytes(jsonStr));
                        T t = (T)deserializer.ReadObject(jsonStrStream);
                        jsonStrStream.Close();
                        jsonStrStream = null;
                        return t;
                    }
                }
                else
                {
                    return default(T);
                }
            }
        }
    }
}
