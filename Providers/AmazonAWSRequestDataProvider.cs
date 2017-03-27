using BookSearchAPI.Models;
using System.IO;
using System.Collections.Generic;

namespace BookSearchAPI.Providers
{
    public static class AmazonAWSRequestDataProvider
    {
        public static AmazonAWSRequestData Provide(string pathToAwsDataConfig)
        {
            return CreateAWSRequestData(pathToAwsDataConfig);
        }

        private static AmazonAWSRequestData CreateAWSRequestData(string pathToAwsDataConfig)
        {
            StreamReader reader = new StreamReader(new FileStream(pathToAwsDataConfig, FileMode.Open));
            string[] lines = reader.ReadToEnd().Split(new char[]{'\n'});

            Dictionary<string, string> data = new Dictionary<string, string>();
            foreach(string line in lines)
            {
                string[] keyVal = line.Split(new char[] {'='});
                data.Add(keyVal[0], keyVal[1]);
            }

            string key;
            string id;
            string tag;
            string url;
            data.TryGetValue("SecretKey", out key);
            data.TryGetValue("URL", out url);
            data.TryGetValue("AssociateTag", out tag);
            data.TryGetValue("AWSAccessKeyId", out id);
            return new AmazonAWSRequestData(key, url, tag, id);
        }
    }
}