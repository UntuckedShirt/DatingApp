using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// all of this is what is returned when the user logs in/ registers
namespace API.DTOs
{
    
    public class UserDto
    {
        public string username { get; set; }

        public string Token { get; set; }
    }
}