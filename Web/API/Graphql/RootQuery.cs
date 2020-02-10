using System;
using GraphQL.Types;
using Web.Api.Database;
using System.Linq;

namespace Web.Api.Graphql
{
    public partial class RootQuery : ObjectGraphType
    {
        public RootQuery(StoreContext db)
        {
            Name = "RootQuery";
            InitializeAuthorQuery(db);
        }
    }
}