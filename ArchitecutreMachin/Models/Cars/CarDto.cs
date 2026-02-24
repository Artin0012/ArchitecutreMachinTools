using System.ComponentModel.DataAnnotations;

namespace ArchitecutreMachin.Models.Cars
{
    public class CarDto
    {
        [Required]
        public string Name { get; set; }
        public List<int> FeatureIds { get; set; } = new();
    }
}
