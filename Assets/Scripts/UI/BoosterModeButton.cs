using System;
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
        private bool m_BoosterMode;
        [Inject]
        public void Construct(IStatsService statsService)
        {
            m_StatsService = statsService;
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
                if (m_BoosterMode)
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
            m_BoosterMode = true;
            Transition(m_BoosterMode);
            gameObject.SetActive(m_BoosterMode);
        }

        public void Disable()
        {
            m_BoosterMode = false;
            Transition(m_BoosterMode);
            gameObject.SetActive(m_BoosterMode);
        }
    }
}