using System;
using UI.Abstract;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Gameplay
{
    public sealed class UIMainMenuView : UIView
    {
        [SerializeField] private Button _startPlayBtn;

        public Button StartPlayBtn => _startPlayBtn;
    }
}