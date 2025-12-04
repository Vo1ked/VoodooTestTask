using System;
using Gameplay.Data;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class BrushItem : MonoBehaviour
    {
        [SerializeField] private Button m_Button;
        [SerializeField] private Transform m_brushContainer;
        [SerializeField] private Image m_backgroundImage;
        private BrushMenu m_BrushMenu;
        [field: SerializeField] public BrushItemData BrushItemData { get; private set; }
    

        public void Initialize(BrushItemData brushItem, Action<BrushItemData> onClick)
        {
            if (m_BrushMenu != null)
            {
                Destroy(m_BrushMenu.gameObject);
            }
            BrushItemData = brushItem;

            m_BrushMenu = Instantiate(brushItem.Brush, m_brushContainer);
            m_BrushMenu.SetNewColor(brushItem.Color);
            SetBackgroundColor(brushItem.Color);
            m_Button.onClick.RemoveAllListeners();
            m_Button.onClick.AddListener(() => onClick?.Invoke(BrushItemData));
        }

        public void Hide()
        {
            m_BrushMenu.gameObject.SetActive(false);
        }

        public void Show()
        {
            m_BrushMenu.gameObject.SetActive(true);
        }

        public void ChangeBrushColor(Color color)
        {
            if (m_BrushMenu == null)
                return;
            m_BrushMenu.SetNewColor(color);
        }

        public void SetBackgroundColor(Color color)
        {
            m_backgroundImage.color = new Color(color.r, color.g, color.b,m_backgroundImage.color.a);
        }
    }
}