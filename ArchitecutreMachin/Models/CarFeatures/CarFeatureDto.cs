using _2.Application.Entities;
using ArchitecutreMachin.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace ArchitecutreMachin.Models.CarFeatures
{
    public class CarFeatureDto 
    {
        public string Color { get; set; }
        [Required]
        public string Title { get; set; }

        [Required]
        public int Rank { get; set; }

        [Range(0, 100)]
        public int HealthyBody { get; set; }

        [Required]
        public int CarId { get; set; }
    }
}
