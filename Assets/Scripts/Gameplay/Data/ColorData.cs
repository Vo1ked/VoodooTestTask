using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Color", menuName = "Data/Color", order = 1)]
public class ColorData : ScriptableObject
{
	[field: SerializeField] public int ColorId { get; private set; }
	public List<Color> 		m_Colors;
}
