using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2.Application.Entities
{
    public class Car : BaseEntity
    {
        [Required]
        public string Name { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public ICollection<CarFeature> Features { get; set; } = new List<CarFeature>();
    }
}
