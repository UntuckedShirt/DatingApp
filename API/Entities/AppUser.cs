using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class AppUser
    {
        // create an auto prop by using prop keyword
        // then you tell its type in the ID field
        // set to Id
        public int Id { get; set; }
        // public means get or set from anywhere in
        // application
        // protected can be accessed from either this 
        // class or any classes that inherits this
        // class. Private means its only accessible
        // from the class in itself
        public string UserName { get; set; }
        // helps in creation of salted passwords
        public byte[] PasswordHash{ get; set; }

        public byte[] PasswordSalt{ get; set; }

    }
}