using System.Collections.Generic;

namespace Interfaces.Services
{
    public interface IFeaturesService
    {
        public IReadOnlyList<IFeature> GetFeatures();
        public void ChangeFeatureState(Feature feature, bool state);
        public bool GetFeatureState(Feature feature);
    }

    public enum Feature
    {
        BrushSelect,
        BoosterPlayMode
    }
}