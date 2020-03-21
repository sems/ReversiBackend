using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ReversiApp.Models;

namespace ReversiApp.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the User class
    public class User : IdentityUser
    {
        [Display(Name = "Unique token")]
        public string Token { get; set; }
        [Display(Name = "Game ID")]
        public int? SpelId { get; set; }
        [Display(Name = "Game")]
        public Spel Spel { get; set; }

        public bool? Archived { get; set; }
    }
}
