using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DT191GProjekt.Models
{
    public class Art
    {
        public int ArtID { get; set; }

        [Required(ErrorMessage = "Title is required!")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Description is required!")]
        public string? Description { get; set; }

        [Display(Name = "Image")]
        public string? ImageName { get; set; }

        [Display(Name = "Alt text (description of image)")]
        public string? AltText { get; set; }

        [NotMapped]
        [Display(Name = "Image upload")]
        public IFormFile? ImageFile { get; set; }
    }
}
