using ArchitecutreMachin.Models.Base;

namespace ArchitecutreMachin.Models.CarFeatures
{
    public class CarFeatureSelectDto : BaseDto
    {
        public string Color { get; set; } 
        public string Title { get; set; }
        public int Rank { get; set; }
        public int HealthyBody { get; set; }
        public int CarId { get; set; }
    }
}
