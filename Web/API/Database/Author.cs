using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web.Api.Database
{
    public class Author
    {
        [Key] public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfileImagePath { get; set; }
        public string FacebookProfileLink { get; set; }
        public string LinkedInProfileLink { get; set; }
    }
}