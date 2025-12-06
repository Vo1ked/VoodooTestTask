using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gameplay.Data
{
    [CreateAssetMenu(fileName = "BoosterLevelsConfig", menuName = "Config/BoosterLevelsConfig", order = 1)]
    public class BoosterLevelsConfig :  ScriptableObject
    {
        /// <summary>
        /// booster set used at creating new level
        /// </summary>
        [SerializeField] private PowerUpData[] m_BasePowerUps = Array.Empty<PowerUpData>();
        [SerializeField] private BoosterLevel[] m_boosterLevels = Array.Empty<BoosterLevel>();
        public Dictionary<int, BoosterLevel> GetBoosterLevels => m_boosterLevels.ToDictionary(x => x.Id);
    }

    [Serializable]
    public class BoosterLevel
    {
        public int Id => m_Id;
        [SerializeField] private int m_Id;

        public IReadOnlyList<PowerUpData> PowerUps => m_PowerUps; 
        [SerializeField] private PowerUpData[] m_PowerUps;

        public BoosterLevel(int id, PowerUpData[] powerUp)
        {
            m_Id = id;
            m_PowerUps = powerUp;
        }
    }
}