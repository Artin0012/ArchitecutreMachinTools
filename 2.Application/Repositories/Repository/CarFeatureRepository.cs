using _2.Application.Context;
using _2.Application.Entities;
using _2.Application.Repositories.Interfaces;
using _3.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3.Infrastructure.Repository
{
    public class CarFeatureRepository : GenericRepository<CarFeature>, ICarFeatureRepository
    {
        public CarFeatureRepository(ApplicationDbContext context) : base(context)
        {

        }
    }
}
