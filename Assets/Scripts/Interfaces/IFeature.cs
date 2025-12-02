using Interfaces.Services;

namespace Interfaces
{
    public interface IFeature
    {
        public Feature m_Name { get; }
        public bool State { get; }

        public void Enable();
        public void Disable();
    }
}