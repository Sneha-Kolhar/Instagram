using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;// to inclue key
using System.Linq;
using System.Threading.Tasks;

namespace SocialSite.Models
{
    public class Users
    {
        [Key] // consider id as a primary key
        public int Id { get; set; }
        public string EmailId { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
       
    }
}
