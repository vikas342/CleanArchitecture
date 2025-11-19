using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class ApplicationUser : IdentityUser
    {

        //Coustom fiels will be added here
        public string Role { get; set; }
    }
}
