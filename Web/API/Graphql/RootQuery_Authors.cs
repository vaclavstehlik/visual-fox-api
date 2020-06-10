using System;
using System.Linq;
using GraphQL.Types;
using Web.Api.Database;

namespace Web.Api.Graphql
{
    public partial class RootQuery
    {
        protected void InitializeAuthorQuery(StoreContext db)
        {
            Field<AuthorType>(
                "author",
                arguments: new QueryArguments(new QueryArgument<IdGraphType> {Name = "id", Description = "The ID of the Author."}),
                resolve: context =>
                {
                    var id = context.GetArgument<Guid>("id");
                    var author = db.Authors.FirstOrDefault(i => i.Id.Equals(id));
                    return author;
                });

            Field<ListGraphType<AuthorType>>(
                "authors",
                resolve: context =>
                {
                    var authors = db.Authors;
                    return authors;
                });

            Field<VisualizationType>(
                "visualization",
                arguments: new QueryArguments(new QueryArgument<IdGraphType> {Name = "id", Description = "The ID of the Visualization."}),
                resolve: context =>
                {
                    var id = context.GetArgument<Guid>("id");
                    var visualizations = db.Visualizations.FirstOrDefault(i => i.Id.Equals(id));
                    return visualizations;
                });

            Field<ListGraphType<VisualizationType>>(
                "visualizations",
                resolve: context =>
                {
                    var visualizations = db.Visualizations;
                    return visualizations;
                });
        }
    }
}