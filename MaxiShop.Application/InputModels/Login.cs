using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxiShop.Application.InputModels
{
    public class Login
    {
        [EmailAddress]
        public string EmailAddress { get; set; }

        [PasswordPropertyText]
        public string Password { get; set; }
    }
}
