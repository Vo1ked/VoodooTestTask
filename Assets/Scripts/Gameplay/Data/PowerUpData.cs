using UnityEngine;

[CreateAssetMenu(fileName = "PowerUp", menuName = "Data/PowerUp", order = 1)]
public class PowerUpData : ScriptableObject
{
	public int 		m_Probability = 1;
	public PowerUp 	m_Prefab;
}
