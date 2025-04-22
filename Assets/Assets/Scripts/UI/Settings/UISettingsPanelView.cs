using System;
using UI.Abstract;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Settings
{
    public class UISettingsPanelView : UIView
    {
        [SerializeField] private Button _returnBtn;
        [SerializeField] private Button _menuBtn;
        
        public IObservable<Unit> OnReturnBtnClick => _returnBtn.OnClickAsObservable();
        public IObservable<Unit> OnMenuBtnClick => _menuBtn.OnClickAsObservable();
    }
}