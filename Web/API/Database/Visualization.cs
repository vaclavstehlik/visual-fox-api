using System;
using System.ComponentModel.DataAnnotations;

namespace Web.Api.Database
{
    public class Visualization
    {
        [Key] public Guid Id { get; set; }
        public string Identifier { get; set; }
    }
}
