using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Models;
using RestSharp;
using RestSharp.Authenticators;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Portfolio.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {

            return View();
        }

        public IActionResult Projects()
        {

            return View();
        }

        public JObject[] GetProjects()
        {
            string authKey = Authentication.basicAuthKey;

            var client = new RestClient("https://api.github.com");
            var request = new RestRequest("/users/rtmath/starred");
            request.AddParameter("client_id", Authentication.AuthId);
            request.AddParameter("client_secret", Authentication.Secret);
            request.AddHeader("User-Agent", "rtmath");
            request.AddHeader("username", authKey);
            request.AddHeader("Accept", "application/vnd.github.v3+json");
            request.AddHeader("Accept", "application/vnd.github.inertia-preview+json");

            var response = new RestResponse();
            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(client, request) as RestResponse;
            }).Wait();
            JObject[] jsonResponse = JsonConvert.DeserializeObject<JObject[]>(response.Content);
            return jsonResponse;
        }

        public static Task<IRestResponse> GetResponseContentAsync(RestClient theClient, RestRequest theRequest)
        {
            var tcs = new TaskCompletionSource<IRestResponse>();
            theClient.ExecuteAsync(theRequest, response => {
                tcs.SetResult(response);
            });
            return tcs.Task;
        }
    }
}
