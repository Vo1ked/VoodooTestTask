using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gameplay.Data
{
    [CreateAssetMenu(fileName = "Brush", menuName = "Data/BrushesSelect", order = 1)]
    public class BrushSelectData :  ScriptableObject
    {
        [SerializeField] private List<BrushData> m_BrushesData;
        public IReadOnlyDictionary<int,BrushData> Brushes =>  m_BrushesData.ToDictionary(x => x.BrushID);
        [SerializeField] private List<ColorData> m_ColorsData;
        public IReadOnlyDictionary<int,ColorData> Colors =>  m_ColorsData.ToDictionary(x => x.ColorId);
    }
}