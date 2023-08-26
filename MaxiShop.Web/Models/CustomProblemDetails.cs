using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace MaxiShop.Web.Models
{
    public class CustomProblemDetails : ProblemDetails
    {
        public IDictionary<string, string[]> Errors { get; set; } = new Dictionary<string, string[]>();
    }
}
