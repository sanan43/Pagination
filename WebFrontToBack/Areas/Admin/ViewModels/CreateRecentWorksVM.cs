using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebFrontToBack.Areas.Admin.ViewModels
{
    public class CreateRecentWorksVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Bos ola bilmez"), MaxLength(50, ErrorMessage = "Uzunluq maks 50 simvol olmalidir")]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required, NotMapped]
        public IFormFile Photo { get; set; }
    }
}
