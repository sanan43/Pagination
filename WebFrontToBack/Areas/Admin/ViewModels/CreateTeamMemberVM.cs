using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebFrontToBack.Areas.Admin.ViewModels
{
    public class CreateTeamMemberVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Bos ola bilmez"), MaxLength(50, ErrorMessage = "Uzunluq maks 50 simvol olmalidir")]
        public string FullName { get; set; }
        [Required]
        public string Profession { get; set; }
        
        [Required, NotMapped]
        public IFormFile Photo { get; set; }
    }
}
