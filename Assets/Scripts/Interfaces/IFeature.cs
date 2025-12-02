using Interfaces.Services;

namespace Interfaces
{
    public interface IFeature
    {
        public Feature Name { get; }
        public bool State { get; }

        public void Enable();
        public void Disable();
    }
}