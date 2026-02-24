using ArchitecutreMachin.Models.Base;
using ArchitecutreMachin.Models.CarFeatures;

namespace ArchitecutreMachin.Models.Cars
{
    public class CarSelectDto : BaseDto
    {

        public string Name { get; set; }
        public Guid UserId { get; set; }

        public List<CarFeatureSelectDto> Features { get; set; }
    }
}
