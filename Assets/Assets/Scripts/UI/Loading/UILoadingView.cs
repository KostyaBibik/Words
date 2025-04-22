using TMPro;
using UI.Abstract;
using UnityEngine;

namespace UI.Loading
{
    public sealed class UILoadingView : UIView
    {
        [SerializeField] private TextMeshProUGUI _text;

        public void UpdateProgress(string phaseName) =>
            _text.text = phaseName;
    }
}