using System.Collections.Generic;
using System.IO;

namespace BookSearchAPI
{
    public class BookSearchWebAppConfiguration
    {
        private Dictionary<string, string> config = new Dictionary<string, string>();

        public void LoadNewConfiguration(string pathToConfigurationFile)
        {
            config.Clear();
            StreamReader reader = new StreamReader(new FileStream(pathToConfigurationFile, FileMode.Open));
            string[] keyValuePairs = reader.ReadToEnd().Split(new char[]{'\n'});
            foreach(string keyValuePair in keyValuePairs)
            {
                string[] keyValue = keyValuePair.Split(new char[] {'='});
                config.Add(keyValue[0].Trim(), keyValue[1].Trim());
            }
        }

        public string GetValue(string key)
        {
            string value;
            if(config.TryGetValue(key, out value))
            {
                return value;
            }
            return null;
        }
    }
}