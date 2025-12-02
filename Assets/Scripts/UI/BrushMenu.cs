using System.Collections.Generic;
using UnityEngine;

public class BrushMenu : MonoBehaviour {
    
    [field:SerializeField] private int BrushId;
    public List<GameObject> m_BrushParts;
    private List<Renderer> m_Renderers = new List<Renderer>();

    private void Awake()
    {
        foreach (var t in m_BrushParts)
        {
            m_Renderers.Add(t.GetComponent<Renderer>());
        }
    }

    public void SetNewColor(Color _Color)
    {
        foreach (var t in m_Renderers)
        {
            t.material.color = _Color;
        }
    }
}
