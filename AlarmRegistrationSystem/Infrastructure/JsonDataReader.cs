using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AlarmRegistrationSystem.Infrastructure
{
    public static class JsonDataReader
    {
        public static T ReadJson <T>(string path)
        {
            T tmp = default(T);
            try
            {
                string jsonData = File.ReadAllText(path);
                tmp = JsonConvert.DeserializeObject<T>(jsonData);
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return tmp;
        }
    }
}
