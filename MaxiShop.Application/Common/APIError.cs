using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxiShop.Application.Common
{
    public class APIError
    {
        private string Description { get; set; }

        public APIError(string description)
        {
            Description = description;
        }
    }
}
