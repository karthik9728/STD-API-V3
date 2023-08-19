using MaxiShop.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxiShop.Domain.Models
{
    public class Brand : BaseModel
    {
        public string Name { get; set; }

        public int EstablishedYear { get; set; }
    }
}
