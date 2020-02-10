using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Web.Api.Database;

namespace Web.API
{
    public class Seeder
    {
        public static void Initialize(StoreContext context)
        {
            context.Database.EnsureCreated();

            // Data was already seeded.
            if (context.Authors.Any())
            {
                return;
            }

            context.Authors.AddRange(
                new Author
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Václav",
                    LastName = "Stehlík",
                    ProfileImagePath = "vaclav_stehlik/profile_image.png",
                    FacebookProfileLink = "https://www.facebook.com/vasa.stehlik",
                    LinkedInProfileLink = "https://www.linkedin.com/in/v%C3%A1clav-stehl%C3%ADk-9b0603148/"
                });
            context.SaveChanges();
        }
    }
}