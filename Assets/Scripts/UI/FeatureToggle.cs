using System;
using Interfaces.Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class FeatureToggle : MonoBehaviour
    {
        [SerializeField] private TMP_Text m_Text;
        [SerializeField] private Toggle m_Toggle;
        private Feature m_Feature;
        
        public void Initialize(Feature feature, bool state, Action<Feature,bool> callback)
        {
            m_Feature = feature;
            m_Text.text = feature.ToString();
            m_Toggle.isOn = state;
            m_Toggle.onValueChanged.AddListener(newState => callback?.Invoke(m_Feature, newState));
        }
    }
}