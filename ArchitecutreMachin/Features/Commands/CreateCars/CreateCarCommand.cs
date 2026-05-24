using MediatR;

namespace ArchitecutreMachins.Features.Commands.CreateCars
{
    public class CreateCarCommand : IRequest<int>
{
        public string Name { get; set; }

        public List<int> FeatureIds { get; set; }
            = new();
    }
}
