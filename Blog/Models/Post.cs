using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace Blog.Models
{
    
    public class Post
    {

        // Defaults
        // Default Date
        public Post()
        {
            Date = DateTime.Now.Date;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        [DataType(DataType.Text)]
        public string Title { get; set; }

        [Required]
        [StringLength(200)]
        [DataType(DataType.Text)]
        public string Description { get; set; }

        [Required]
        [AllowHtml]
        [DataType(DataType.MultilineText)]
        public string Body { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [ForeignKey("AuthorId")]
        public ApplicationUser Author { get; set; }

        public string AuthorId { get; set; }

        public ICollection<Comment> Comments { get; set; }

        public virtual ICollection<File> Files { get; set; }

    }
}