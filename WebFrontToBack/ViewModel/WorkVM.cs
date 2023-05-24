using WebFrontToBack.Models;

namespace WebFrontToBack.ViewModel
{
    public class WorkVM
    {
       
        public List<WorkCategory> WorkCategories { get; set; }
        public List<WorkService> WorkServices { get; set; }
        public List<RecentWorks> recentWorks { get; set; }

    }

}
