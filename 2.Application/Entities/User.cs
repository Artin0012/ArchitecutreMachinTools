using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2.Application.Entities
{
    public class User : IdentityUser
    {
        [Required]
        public string FullName { get; set; }

        public ICollection<Car> Cars { get; set; } = new List<Car>();
    }
}
