using System;
using Interfaces.Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class BoosterModeButton : View<BoosterModeButton>
    {
        [SerializeField] private TMP_Text m_CurrentLevelText;
        [SerializeField] private Button m_Button;
        
        private IStatsService m_StatsService;
        private IFeaturesService m_FeaturesService;
        
        [Inject]
        public void Construct(IStatsService statsService, IFeaturesService featuresService)
        {
            m_StatsService = statsService;
            m_FeaturesService = featuresService;
            
            SetCurrentLevel(m_StatsService.CurrentBoosterLevel);
            m_Button.onClick.AddListener(PlayButton);
        }
        
        private void SetCurrentLevel(int level)
        {
            m_CurrentLevelText.text = $"Level {level}";
        }

        public void PlayButton()
        {
            GameService.m_BoosterMode = true;
            if (GameService.currentPhase == GamePhase.MAIN_MENU)
                GameService.ChangePhase(GamePhase.LOADING);
        }

        protected override void OnGamePhaseChanged(GamePhase _GamePhase)
        {
            if (_GamePhase == GamePhase.MAIN_MENU)
            {
                if (m_FeaturesService.GetFeatureState(Feature.BoosterPlayMode))
                {
                    Enable();
                }
                else
                {
                    Disable();
                }
            }
            if (_GamePhase != GamePhase.END) 
                return;
            
            GameService.m_BoosterMode = false;
            SetCurrentLevel(m_StatsService.CurrentBoosterLevel);
        }


        public void Enable()
        {
            Transition(true);
            gameObject.SetActive(true);
        }

        public void Disable()
        {
            Transition(false);
            gameObject.SetActive(false);
        }
    }
}