using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    // we need something that validates an endpoint
    public class RegisterDto
    {
        // we need to use data annotations
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}