using Interfaces;
using Interfaces.Services;

namespace Gameplay.Features
{
    public class BrushSelectFeature :  IFeature
    {
        public Feature m_Name => Feature.BrushSelect;

        public bool State => m_State;
        private bool m_State = false;
        public void Enable()
        {
            m_State = true;
        }

        public void Disable()
        {
            m_State = false;
        }
    }
}