using Interfaces.Services;
using UnityEngine;
using Zenject;

namespace UI
{
    public class FeaturesPanel :  View<FeaturesPanel>
    {
        [SerializeField] private FeatureToggle m_TogglePrefab;
        
        private IFeaturesService m_FeaturesService;

        private bool m_visible;
        
        [Inject]
        public void Construct(IFeaturesService featuresService)
        {
            m_FeaturesService = featuresService;
        }

        protected override void Awake()
        {
            base.Awake();
            foreach (var feature in m_FeaturesService.GetFeatures())
            {
                var toggle = Instantiate(m_TogglePrefab, transform);
                toggle.Initialize(feature.Name,feature.State,ChangeState);
            }
        }

        private void ChangeState(Feature feature, bool state)
        {
            m_FeaturesService.ChangeFeatureState(feature, state);
        }

        public void SwitchTransition()
        {
            m_visible = !m_visible;
            Transition(m_visible);
        }
        
    }
}