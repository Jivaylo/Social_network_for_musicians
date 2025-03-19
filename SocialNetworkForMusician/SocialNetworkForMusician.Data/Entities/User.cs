using SocialNetworkForMusician.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNet.Identity.EntityFramework;

namespace SocialNetworkForMusician.Data.Models.Entities
{

    public class User : IdentityUser
    {
        public bool IsArtist { get; set; }
    }
}
