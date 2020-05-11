using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Mvc;
using Web.Interfaces;

namespace Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RecommenderController : Controller
    {
        private readonly IRecommenderService _recommenderService;

        public RecommenderController(IRecommenderService recommenderService)
        {
            _recommenderService = recommenderService;
        }

        [HttpPost("Recommendation")]
        public async Task<IActionResult> GetRecommendation([FromBody] JsonElement recommendationRequest)
        {
            try
            {
                var useCriteria = recommendationRequest.GetProperty("user_criteria");
                var ranking = await _recommenderService.GetRanking(useCriteria);

                return Ok(ranking);
            }
            catch (KeyNotFoundException e)
            {
                throw new ApiProblemDetailsException("Missing data property in the request", (int)HttpStatusCode.UnprocessableEntity);
            }
        }
    }
}
