using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebFrontToBack.Models
{
    public class TeamMember
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Bos ola bilmez"), MaxLength(50, ErrorMessage = "Uzunluq maks 50 simvol olmalidir")]
        public string FullName { get; set; }
        [Required]
        public string Profession { get; set; }
        [Required]
        public string ImagePath { get; set; }
        

    }
}
