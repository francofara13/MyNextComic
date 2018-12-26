using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MyNextComic.Services
{
    public class StringTable
    {
        public string[] ColumnNames { get; set; }
        public string[,] Values { get; set; }
    }
    public class Response
    {
        public Results results { get; set; }
    }
    public class Results
    {
        public Output output1 { get; set; }
    }

    public class Output
    {
        public string type { get; set; }
        public Value value { get; set; }
    }

    public class Value
    {
        public string[] ColumnNames { get; set; }
        public string[] ColumnTypes { get; set; }
        public string[,] Values { get; set; }
    }

    public class RecommenderService
    {
        public async Task<List<int>> GetRecommendation(int userId)
        {
            using (var client = new HttpClient())
            {
                var scoreRequest = new
                {

                    Inputs = new Dictionary<string, StringTable>() {
                        {
                            "input1",
                            new StringTable()
                            {
                                ColumnNames = new string[] {"userid"},
                                Values = new string[,] {  { userId.ToString() }  }
                            }
                        },
                    },
                    GlobalParameters = new Dictionary<string, string>()
                    {
                    }
                };
                const string apiKey = "T+RJskZhi6EEFqd9DNH/nl8KLvci7/pugxgnYAt++/XsFbnqsv9ddupXFmZp7+kXeN95HX2TMa794jHu4fCr9A=="; // Replace this with the API key for the web service
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

                var uri = new Uri("https://ussouthcentral.services.azureml.net/workspaces/51fec8d6f8f445f1a5e16271c6f0069b/services/980dad73eb814dfcbb173eeb0c562ecd/execute?api-version=2.0&details=true");

                var stringScoreRequest = await Task.Run(() => JsonConvert.SerializeObject(scoreRequest));
                var httpContent = new StringContent(stringScoreRequest, Encoding.UTF8, "application/json");

                HttpResponseMessage apiResponse = await client.PostAsync(uri, httpContent);

                if (apiResponse.IsSuccessStatusCode)
                {
                    string stringResponse = await apiResponse.Content.ReadAsStringAsync();
                    JObject result = JObject.Parse(stringResponse);
                    var response = result.ToObject<Response>();

                    List<int> comicsIds = new List<int>();

                    foreach (var id in response.results.output1.value.Values)
                    {
                        comicsIds.Add(Convert.ToInt32(id));
                    }

                    comicsIds.RemoveAt(0);

                    return comicsIds;
                }
                else
                {
                    Console.WriteLine(string.Format("The request failed with status code: {0}", apiResponse.StatusCode));

                    // Print the headers - they include the requert ID and the timestamp, which are useful for debugging the failure
                    Console.WriteLine(apiResponse.Headers.ToString());

                    string responseContent = await apiResponse.Content.ReadAsStringAsync();
                    Console.WriteLine(responseContent);

                    return null;
                }
            }
        }
    }
}
