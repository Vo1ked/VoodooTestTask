using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class BoosterModeButton : View<BoosterModeButton>
    {
        [SerializeField] private TMP_Text m_CurrentLevelText;
        [SerializeField] private Button m_Button;

        public void SetCurrentLevel(int level)
        {
            m_CurrentLevelText.text = $"Level {level}";
        }
        

        public void Enable()
        {
            Transition(true);
            gameObject.SetActive(true);
        }

        public void Disable()
        {
            Transition(false);
            gameObject.SetActive(false);
        }
    }
}