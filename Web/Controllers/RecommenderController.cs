using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Web.Interfaces;
using Web.Models;

namespace Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RecommenderController : Controller
    {
        private IConfiguration Configuration { get; }
        private readonly IRecommenderService _recommenderService;

        public RecommenderController(IConfiguration configuration, IRecommenderService recommenderService)
        {
            Configuration = configuration;
            _recommenderService = recommenderService;
        }

        [HttpPost("Recommendation")]
        public async Task<IActionResult> GetRecommendation([FromBody] JsonElement criteria)
        {
            var content = new StringContent(criteria.ToString(), Encoding.UTF8, "application/json");
            using var client = new HttpClient();

            var url = Configuration.GetValue<string>("Recommender:Url");
            var result = await client.PostAsync($"{url}/dataset/main", content);
            var ranking = await result.Content.ReadAsStringAsync();

            return Ok(ranking);
        }
    }
}