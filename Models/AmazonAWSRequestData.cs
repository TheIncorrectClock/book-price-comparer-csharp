namespace BookSearchAPI.Models
{
    public class AmazonAWSRequestData
    {
        public string SecretKey
        {
            get;
        }

        public string URL
        {
            get;
        }

        public string AssociateTag
        {
            get;
        }

        public string AWSAccessKeyId
        {
            get;
        }

        public AmazonAWSRequestData(string secretKey, string url, string associateTag, string awsAccessKeyId)
        {
            this.SecretKey = secretKey;
            this.URL = url;
            this.AssociateTag = associateTag;
            this.AWSAccessKeyId = awsAccessKeyId;
        }
    }
}