using System;
using System.Collections.Generic;
using RestSharp;

namespace omnigage_bulk_delete_example
{
    public class Program
    {
        static void Main(string[] args)
        {
            // Add one or more contact IDs
            List<string> contacts = new List<string> { };

            // Set token retrieved from Account -> Developer -> API Tokens
            string tokenKey = "";
            string tokenSecret = "";

            // Retrieve from Account -> Settings -> General -> "Key" field
            string accountKey = "";

            // API host path, only change if using sandbox
            string host = "https://api.omnigage.io/api/v1/";

            // Buld the serialized list of contacts
            string requestInstances = "";
            foreach (var contactId in contacts)
            {
                if (requestInstances != "")
                {
                    requestInstances += ",";
                }

                requestInstances += @"{
                    ""type"": ""contacts"",
                    ""id"": """ + contactId + @"""
                }";
            }

            // Create schema
            string requestContent = @"{
                ""data"": [" + requestInstances + @"]
            }";

            // Build basic authorization
            string authorization = createAuthorization(tokenKey, tokenSecret);

            // Bulk request header
            string bulkRequestHeader = "application/vnd.api+json;ext=bulk";

            var client = new RestClient(host + "contacts");
            var request = new RestRequest(Method.DELETE);
            request.AddHeader("Accept", bulkRequestHeader);
            request.AddHeader("Content-Type", bulkRequestHeader);
            request.AddHeader("X-Account-Key", accountKey);
            request.AddHeader("Authorization", "Basic " + authorization);
            request.AddParameter(bulkRequestHeader, requestContent, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                Console.WriteLine("Success");
            }
            else
            {
                Console.WriteLine("Failure");
                Console.WriteLine(response.Content);
            }
        }

        /// <summary>
        /// Create Authorization token following RFC 2617 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="secret"></param>
        /// <returns>Base64 encoded string</returns>
        static string createAuthorization(string key, string secret)
        {
            byte[] authBytes = System.Text.Encoding.UTF8.GetBytes($"{key}:{secret}");
            return System.Convert.ToBase64String(authBytes);
        }
    }
}