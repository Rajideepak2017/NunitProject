using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NunitSelenium.Utilities
{
    public class JsonReader
    {
        public JsonReader() { }
        public String dataextract(string tokenname)
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;

            string filePath = Path.Combine(basePath, "Utilities", "Testdata.json");

            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Test data file not found at: {filePath}");



            string jsonContent = File.ReadAllText(filePath); 
            var jsonObject = JToken.Parse(jsonContent);
            var token = jsonObject.SelectToken(tokenname);
            if (token == null)
                throw new Exception($"Token '{tokenname}' not found in JSON.");

            return token.Value<string>();



        }
    }
}
