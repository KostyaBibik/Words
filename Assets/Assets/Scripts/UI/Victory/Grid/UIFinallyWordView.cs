﻿using TMPro;
using UI.Abstract;
using UnityEngine;

namespace UI.Victory.Grid
{
    public class UIFinallyWordView : UIView
    {
        [SerializeField] private TextMeshProUGUI _text;

        public void UpdateText(string text) =>
            _text.text = text;
    }
}