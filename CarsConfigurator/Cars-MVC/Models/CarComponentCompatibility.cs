using Dao.Models;

namespace Cars_MVC.Models
{
    public class CarComponentCompatibility
    {
        public int CarComponentId1 { get; set; }
        public int CarComponentId2 { get; set; }

        public virtual CarComponent CarComponentId1Navigation { get; set; } = null!;
        public virtual CarComponent CarComponentId2Navigation { get; set; } = null!;

    }

}