using _2.Application.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3.Infrastructure.Services
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}
