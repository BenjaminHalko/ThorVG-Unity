using System.Threading.Tasks;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine.Networking;
// ReSharper disable InconsistentNaming

namespace Lottie.Editor
{
    internal static class LottieAPI
    {
        private const string BaseUrl = "https://graphql.lottiefiles.com/";
        
        // RESPONSES
        
        private struct GraphQLResponse<T>
        {
            public T data { get; set; }
        }

        public struct LottieEdge
        {
            public LottieNode node { get; set; }
            public string cursor { get; set; }
        }

        public struct LottieNode
        {
            public string jsonUrl { get; set; }
            public string name { get; set; }
            
            public LottieUser createdBy { get; set; }
        }

        public struct LottieUser
        {
            public string firstName { get; set; }
        }

        private struct SearchPublicAnimationsQuery
        {
            public SearchPublicAnimationsResponse searchPublicAnimations;
        }
        
        public struct SearchPublicAnimationsResponse
        {
            public LottieEdge[] edges { get; set; }
            public int totalCount { get; set; }
        }
        
        // METHODS

        private static async Task<T> MakeRequest<T>(object query)
        {
            var jsonQuery = new
            {
                query = $"query {query}"
            };
            var request = new UnityWebRequest(BaseUrl, "POST")
            {
                uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(jsonQuery))),
                downloadHandler = new DownloadHandlerBuffer()
            };
            request.SetRequestHeader("Content-Type", "application/json");
            
            var operation = request.SendWebRequest();
            var tcs = new TaskCompletionSource<bool>();

            operation.completed += _ => tcs.SetResult(true);
            await tcs.Task;

            if (request.result != UnityWebRequest.Result.Success)
            {
                throw new System.Exception(request.error);
            }
            
            var text = request.downloadHandler.text;
            var response = JsonConvert.DeserializeObject<GraphQLResponse<T>>(text).data;
            return response;
        }
        
        public static async Task<SearchPublicAnimationsResponse> Search(string search, int limit, string startCursor = "")
        {
            var query = @"
            {
              searchPublicAnimations(
                first: " + limit + @"
                query: """ + search + @"""
              ) {
                edges {
                  node {
                    jsonUrl
                    createdBy {
                        firstName
                    },
                    name
                  }
                  cursor
                }
                totalCount
              }
            }";
            
            return (await MakeRequest<SearchPublicAnimationsQuery>(query)).searchPublicAnimations;
        }
    }
}