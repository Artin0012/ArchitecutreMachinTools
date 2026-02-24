using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace _2.Application.Entities
{
    public class CarFeature : BaseEntity
    {
        [Required]
        public string Color { get; set; }

        [Required]
        public string Title { get; set; }
        [Required]
        public int Rank { get; set; }
        [Range(0, 100)]
        public int HealthyBody { get; set; }

        public ICollection<Car> Cars { get; set; } = new List<Car>();
    }
}
