using GraphQL.Types;
using Web.Api.Database;

namespace Web.Api.Graphql
{
    public class VisualizationType: ObjectGraphType<Visualization>
    {
        public VisualizationType()
        {
            Name = "Visualization";
            Field(x => x.Id, type: typeof(IdGraphType)).Description("The ID of the visualization.");
            Field(x => x.Identifier).Description("String identifier of the visualization");
        }
    }
}
