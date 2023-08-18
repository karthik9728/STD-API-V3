using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxiShop.Application.DTO.Category
{
    public class UpdateCategoryDto
    {
        public int Id { get; set; }

        public string CategoryName { get; set; } = string.Empty;
    }
}
