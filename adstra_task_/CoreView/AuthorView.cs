using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace adstra_task_.CoreView
{
    public class AuthorView
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [Display(Name = "User Id")]
        public string UserId { get; set; }
        [Required]
        [Display(Name = "User Name ")]
        public string UserName { get; set; }
        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }
        [Required]
        [Display(Name = "Profile Picture Url")]
        public IFormFile ProfileImageUrl { get; set; }
        public string Bio { get; set; }
        public string FaceBook { get; set; }
        public string Instgram { get; set; }
        public string Twitter { get; set; }
    }
}
