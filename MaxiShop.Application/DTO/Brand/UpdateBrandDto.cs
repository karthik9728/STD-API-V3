﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxiShop.Application.DTO.Brand
{
    public class UpdateBrandDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int EstablishedYear { get; set; }
    }
}
