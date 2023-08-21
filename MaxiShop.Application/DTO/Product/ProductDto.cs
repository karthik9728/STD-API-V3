using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxiShop.Application.DTO.Product
{
    public class ProductDto
    {
        public int Id { get; set; }

        [DisplayName("Category Id")]
        public int CategoryId { get; set; }

        [DisplayName("Category Name")]
        public string Category { get; set; }

        [DisplayName("Brand Id")]
        public int BrandId { get; set; }

        [DisplayName("Brand Name")]
        public string Brand { get; set; }

        public string Name { get; set; }

        public string Specification { get; set; }

        public double Price { get; set; }
    }
}
