using UnityEngine;

public class SkinData : ScriptableObject
{
    [field: SerializeField] public int SkinID {get; private set; }
    public ColorData Color;
    public BrushData Brush;
}