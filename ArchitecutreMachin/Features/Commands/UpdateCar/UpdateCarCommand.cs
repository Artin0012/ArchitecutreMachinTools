using MediatR;

namespace ArchitecutreMachins.Features.Commands.UpdateCar
{
    public class UpdateCarCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<int> FeatureIds { get; set; }
            = new();
    }
}
