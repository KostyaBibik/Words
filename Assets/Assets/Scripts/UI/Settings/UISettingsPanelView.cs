using System;
using Lean.Gui;
using UI.Abstract;
using UniRx;
using UnityEngine;

namespace UI.Settings
{
    public class UISettingsPanelView : UIView
    {
        [SerializeField] private LeanButton _returnBtn;
        [SerializeField] private LeanButton _menuBtn;
        
        public IObservable<Unit> OnMenuBtnClick => _menuBtn.OnClick.AsObservable();
        public IObservable<Unit> OnReturnBtnClick => _returnBtn.OnClick.AsObservable();
    }
}