using System;
using UnityEngine;

namespace Gameplay.Data
{
    [Serializable]
    public struct BrushItemData
    {
        public readonly BrushMenu Brush;
        public readonly Color Color;
        public readonly int BrushId;
        public readonly int ColorId;

        public BrushItemData(BrushMenu brush, Color color, int brushId, int colorId)
        {
            Brush = brush;
            Color = color;
            BrushId = brushId;
            ColorId = colorId;
        }
    }
}