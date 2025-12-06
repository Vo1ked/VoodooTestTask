using Interfaces;
using Interfaces.Services;
using UI;

namespace Gameplay.Features
{
    public class BoosterModeFeature : IFeature
    {
        public Feature Name => Feature.BoosterPlayMode;
        public bool State => m_State;
        private bool m_State = false;
        public void Enable()
        {
            m_State = true;
            BoosterModeButton.Instance.Enable();
        }

        public void Disable()
        {
            m_State = false;
            BoosterModeButton.Instance.Disable();
        }
    }
}