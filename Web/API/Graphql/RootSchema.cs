using GraphQL.Types;
using GraphQL;
using Web.Api.Database;

namespace Web.Api.Graphql
{
    public class RootSchema : Schema
    {
        public RootSchema(IDependencyResolver resolver) : base(resolver)
        {
            Query = resolver.Resolve<RootQuery>();
        }
    }
}