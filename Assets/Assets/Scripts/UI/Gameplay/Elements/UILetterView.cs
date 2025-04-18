using TMPro;
using UnityEngine;

namespace UI.Gameplay.Elements
{
    public class UILetterView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;

        public void Setup(char character)
        {
            _text.text = character.ToString();
        }
    }
}