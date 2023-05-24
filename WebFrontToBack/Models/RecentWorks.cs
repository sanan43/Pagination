using System.ComponentModel.DataAnnotations;

namespace WebFrontToBack.Models
{
    public class RecentWorks
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Bos ola bilmez"), MaxLength(50, ErrorMessage = "Uzunluq maks 50 simvol olmalidir")]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string ImagePath { get; set; }
    }
}
