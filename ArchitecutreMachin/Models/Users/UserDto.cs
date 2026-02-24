using System.ComponentModel.DataAnnotations;

namespace ArchitecutreMachin.Models.Users
{
    public class UserDto
    {
        [Required]
        public string FullName { get; set; }
    }
}
