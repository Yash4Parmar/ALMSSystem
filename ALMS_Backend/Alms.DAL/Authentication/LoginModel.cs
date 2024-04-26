using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alms.DAL.Authentication
{
    public class LoginModel
    {
        [Required(ErrorMessage = "User Name Or Email is required")]
        public string UsernameOrEmail { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
