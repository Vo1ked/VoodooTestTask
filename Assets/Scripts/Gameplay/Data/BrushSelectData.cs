using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Data
{
    [CreateAssetMenu(fileName = "Brush", menuName = "Data/BrushesSelect", order = 1)]
    public class BrushSelectData :  ScriptableObject
    {
        [SerializeField] private List<BrushData> m_BrushesData;
        public IReadOnlyList<BrushData> Brushes =>  m_BrushesData;
        [SerializeField] private List<ColorData> m_ColorsData;
        public IReadOnlyList<ColorData> Colors =>  m_ColorsData;
    }
}