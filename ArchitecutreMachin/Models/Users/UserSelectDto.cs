using ArchitecutreMachin.Models.Base;
using ArchitecutreMachin.Models.Cars;

namespace ArchitecutreMachin.Models.Users
{
    public class UserSelectDto : BaseDto
    {
        public string FullName { get; set; }

        public ICollection<CarSelectDto> Cars { get; set; }
    }
}
