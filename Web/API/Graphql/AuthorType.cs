using GraphQL.Types;
using Web.Api.Database;

namespace Web.Api.Graphql
{
    public class AuthorType : ObjectGraphType<Author>
    {
        public AuthorType()
        {
            Name = "Author";
            Field(x => x.Id, type: typeof(IdGraphType)).Description("The ID of the author.");
            Field(x => x.FirstName).Description("The first name of the author");
            Field(x => x.LastName).Description("The last name of the author");
            Field(x => x.ProfileImagePath).Description("The profile image path slug.");
            Field(x => x.FacebookProfileLink).Description("The Facebook profile url link.");
            Field(x => x.LinkedInProfileLink).Description("The LinkedIn profile url link.");
        }
    }
}