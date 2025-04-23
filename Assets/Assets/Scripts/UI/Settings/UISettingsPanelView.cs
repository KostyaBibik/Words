using System;
using Lean.Gui;
using UI.Abstract;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Settings
{
    public class UISettingsPanelView : UIView
    {
        [Header("References")]
        [SerializeField] private LeanButton _returnBtn;
        [SerializeField] private Button _soundButton;
        [SerializeField] private Image _sounds;
        [Header("Sprites")] 
        [SerializeField] private Sprite _soundOn;
        [SerializeField] private Sprite _soundOff;

        private bool _soundStatus = true;
        
        public IObservable<Unit> OnReturnBtnClick => _returnBtn.OnClick.AsObservable();
        public IObservable<Unit> OnSoundsBtnClick => _soundButton.OnClickAsObservable();

        public void SwapSoundSprite()
        {
            _soundStatus = !_soundStatus;
            _sounds.sprite = _soundStatus
                ? _soundOn
                : _soundOff;
        }
    }
}