using System.Collections.Generic;
using Gameplay.Data;
using Interfaces;
using Interfaces.Services;
using UI;
using UnityEngine;

namespace Gameplay.Features
{
    public class BoosterModeFeature : IFeature
    {
        public Feature Name => Feature.BoosterPlayMode;
        public bool State => m_State;
        private bool m_State = false;
        
        private readonly IStatsService m_Stats;
        private readonly BoosterLevelsConfig m_Config;

        public BoosterModeFeature(IStatsService statsService, BoosterLevelsConfig config)
        {
            m_Stats = statsService;
            m_Config = config;
        }
            
        public void Enable()
        {
            m_State = true;
            BoosterModeButton.Instance.Enable();
        }

        public IReadOnlyList<PowerUpData> GetPowerUps()
        {
            return m_Config.BoosterLevels[m_Stats.RealCurrentBoosterLevel].PowerUps;
        }

        public void IncreaseBoosterLevel()
        {
            m_Stats.IncreaseBoostGameLevel();
            m_Stats.RealCurrentBoosterLevel = m_Stats.CurrentBoosterLevel;
            if (m_Stats.CurrentBoosterLevel > m_Config.BoosterLevels.Count)
            {
                m_Stats.RealCurrentBoosterLevel = Random.Range(0, m_Config.BoosterLevels.Count);
            }
        }

        public void Disable()
        {
            m_State = false;
            BoosterModeButton.Instance.Disable();
        }
    }
}