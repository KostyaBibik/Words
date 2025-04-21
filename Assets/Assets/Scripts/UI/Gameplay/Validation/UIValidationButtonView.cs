using System;
using UI.Abstract;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Gameplay.Validation
{
    public class UIValidationButtonView : UIView
    {
        [SerializeField] private Button _validateButton;

        private readonly Subject<Unit> _onValidateClicked = new();
        public IObservable<Unit> OnValidateClicked => _onValidateClicked;

        protected override void Awake()
        {
            _validateButton.onClick.AddListener(() => 
                _onValidateClicked.OnNext(Unit.Default)
            );
        }

        private void OnDestroy()
        {
            _onValidateClicked?.OnCompleted();
        }
    }
}