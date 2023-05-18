using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using adstra_task.Core;


namespace adstra_task_.CoreView
{
    public class AuthorPostView
    {
        [Required]
        public int Id { get; set; }
      
        [Display(Name = "User Id")]
        public string UserId { get; set; }
        
        [Display(Name = "User Name ")]
        public string UserName { get; set; }
        
        [Display(Name = "Full Name")]
        public string FullName { get; set; }
        [Required]
        [Display(Name = "Post Category  ")]
        [DataType(DataType.Text)]
        public string postCategory { get; set; }
        [Required]
        [Display(Name = "Post Title")]
        [DataType(DataType.Text)]
        public string PostTitle { get; set; }
        [Required]
        [Display(Name = "Post Descraption")]
        [DataType(DataType.MultilineText)]
        public string PostDescraption { get; set; }
        [Required]
        [Display(Name = "Profile Picture Url")]
        [DataType(DataType.Upload)]
        public IFormFile PostImageUrl { get; set; }
        [Required]
        [Display(Name = "Added Date")]
        public DateTime AddedDate { get; set; }


        //Navigtion Area
        public int AuthorId { get; set; }
        public Authors Authors { get; set; }

        public int CategoryId { get; set; }
        public Category category { get; set; }
    }
}
