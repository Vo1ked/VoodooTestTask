using System.Collections.Generic;
using System.Linq;
using Interfaces;
using Interfaces.Services;
using UnityEngine;

namespace Services
{
    public class FeaturesService : IFeaturesService
    {
        private readonly Dictionary<Feature,IFeature> m_Features;
        public FeaturesService(IEnumerable<IFeature> features)
        {
            m_Features = new Dictionary<Feature, IFeature>();
            foreach (var feature in features)
            {
                m_Features.Add(feature.Name, feature);
                var savedState = PlayerPrefs.GetInt("Feature_" + feature.Name, 1);
                if (savedState is 1)
                {
                    feature.Enable();
                }
                else
                {
                    feature.Disable();
                }
            }
        }
        
        public void ChangeFeatureState(Feature feature, bool state)
        {
            if (!m_Features.TryGetValue(feature, out IFeature featureObj)) 
                return;
            
            if (state)
            {
                featureObj.Enable();
            }
            else
            {
                featureObj.Disable();
            }
            SaveFeature(feature,state);
        }

        public bool GetFeatureState(Feature feature) =>
            m_Features.TryGetValue(feature, out IFeature featureObj) && featureObj.State;
        public IReadOnlyList<IFeature> GetFeatures() => m_Features.Values.ToList();
        private void SaveFeature(Feature feature,bool state)
        {
            PlayerPrefs.SetInt($"Feature_{feature.ToString()}",state?1:0);
        }
    }
}