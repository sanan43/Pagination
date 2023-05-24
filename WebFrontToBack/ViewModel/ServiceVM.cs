using WebFrontToBack.Models;

namespace WebFrontToBack.ViewModel
{
    public class ServiceVM
    {
        public Service services { get; set; }
        public List<Category>? categories { get; set; }
    }
}
