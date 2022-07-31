using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SocialSite.Models
{
    public class Friends
    {
        [Key]
        public int? Id { get; set; }

        [ForeignKey("UserId")]
        //public int? UserId { get; set; }
        public int UserId { get; set; }
        public string Requests { get; set; }
        public string Friend { get; set; }
        //public virtual Users User { get; set; }
    }
}
