using Microsoft.AspNetCore.Identity;
using SocialNetworkForMusician.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;


namespace SocialNetworkForMusician.Data.Models.Entities
{

    public class User : IdentityUser
    {
        public string FullName { get; set; }
    }
}
