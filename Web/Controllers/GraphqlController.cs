using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoWrapper.Filters;
using Web.Api.Graphql;
using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("graphql")]
    [ApiController]
    public class GraphqlController : ControllerBase
    {
        private readonly ISchema _schema;

        public GraphqlController(ISchema schema)
        {
            _schema = schema;
        }

        [HttpPost]
        [AutoWrapIgnore]
        public async Task<ActionResult> Post([FromBody] GraphQLQuery query)
        {
            var inputs = query.Variables.ToInputs();

            var result = await new DocumentExecuter().ExecuteAsync(_ =>
            {
                _.Schema = _schema;
                _.Query = query.Query;
                _.OperationName = query.OperationName;
                _.Inputs = inputs;
            });

            if (result.Errors?.Count > 0)
            {
                return BadRequest();
            }

            return Ok(result);
        }
    }
}