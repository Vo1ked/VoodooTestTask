using UnityEngine;

[CreateAssetMenu(fileName = "Brush", menuName = "Data/Brush", order = 1)]
public class BrushData : ScriptableObject
{
	[field: SerializeField] public int BrushID {get; private set; }
	public GameObject 	    m_BrushPrefab;
	public GameObject 	m_BrushMenuPrefab;
}
