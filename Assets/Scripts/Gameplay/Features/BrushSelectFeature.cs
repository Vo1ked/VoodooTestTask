using Interfaces;
using Interfaces.Services;
using UI;

namespace Gameplay.Features
{
    public class BrushSelectFeature :  IFeature
    {
        public Feature Name => Feature.BrushSelect;

        public bool State => m_State;
        private bool m_State = false;
        public void Enable()
        {
            m_State = true;
            BrushSelectView.Instance.Enable();
        }

        public void Disable()
        {
            m_State = false;
            BrushSelectView.Instance.Disable();
        }
    }
}