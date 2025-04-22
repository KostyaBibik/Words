using System;
using Lean.Gui;
using UI.Abstract;
using UniRx;
using UnityEngine;

namespace UI.Gameplay.Settings
{
    public class UISettingsButtonView : UIView
    {
        [SerializeField] private LeanButton _button;
        
        public IObservable<Unit> OnBtnClick => _button.OnClick.AsObservable();
    }
}