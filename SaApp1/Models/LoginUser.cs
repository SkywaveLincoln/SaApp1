using System;
using System.Data.Entity;
using System.Linq;

namespace SaApp1.Models
{

    public class LoginUser
    {
        public string FullName { get; set; } = "";
        public string Password { get; set; } = "";
    }
}