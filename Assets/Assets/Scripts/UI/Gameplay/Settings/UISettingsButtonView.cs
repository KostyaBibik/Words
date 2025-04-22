using System;
using UI.Abstract;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Gameplay.Settings
{
    public class UISettingsButtonView : UIView
    {
        [SerializeField] private Button _button;
        
        public IObservable<Unit> OnBtnClick => _button.OnClickAsObservable();
    }
}