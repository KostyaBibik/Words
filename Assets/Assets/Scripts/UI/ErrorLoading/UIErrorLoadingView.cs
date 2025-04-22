using TMPro;
using UI.Abstract;
using UnityEngine;

namespace UI.ErrorLoading
{
    public sealed class UIErrorLoadingView : UIView
    {
        [SerializeField] private TextMeshProUGUI _text;

        public void UpdateText(string info) =>
            _text.text = info;
    }
}