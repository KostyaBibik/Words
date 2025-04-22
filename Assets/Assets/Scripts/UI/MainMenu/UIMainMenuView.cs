using Lean.Gui;
using UI.Abstract;
using UnityEngine;
namespace UI.Gameplay
{
    public sealed class UIMainMenuView : UIView
    {
        [SerializeField] private LeanButton _startPlayBtn;

        public LeanButton StartPlayBtn => _startPlayBtn;
    }
}