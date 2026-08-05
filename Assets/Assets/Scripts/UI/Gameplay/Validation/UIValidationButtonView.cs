using System.Threading;
using Cysharp.Threading.Tasks;
using Lean.Gui;
using UI.Abstract;
using UI.Juice;
using UniRx;
using UnityEngine;

namespace UI.Gameplay.Validation
{
    public class UIValidationButtonView : UIView
    {
        [SerializeField] private LeanButton _button;
        [SerializeField] private LeanPulse _errorNotification;

        [Header("Juice")]
        [Tooltip("Element that shakes on a failed check. Falls back to the button itself.")]
        [SerializeField] private RectTransform _shakeTarget;
        [SerializeField] private float _shakeStrength = 22f;
        [SerializeField] private float _shakeDuration = 0.4f;

        private readonly AsyncReactiveCommand<Unit> _onValidateCommand = new();
        private CancellationTokenSource _juiceCts;

        public AsyncReactiveCommand<Unit> OnValidateCommand => _onValidateCommand;

        protected override void Awake()
        {
            base.Awake();

            if (_shakeTarget == null)
                _shakeTarget = transform as RectTransform;

            _button.OnClick.AddListener(() =>
            {
                _onValidateCommand.Execute(Unit.Default);
            });
        }

        private void OnDestroy()
        {
            CancelJuice();
            _onValidateCommand.Dispose();
        }

        public void ShowErrorNotification()
        {
            _errorNotification.Pulse();

            CancelJuice();
            _juiceCts = CancellationTokenSource.CreateLinkedTokenSource(this.GetCancellationTokenOnDestroy());

            UITween.Shake(_shakeTarget, _shakeStrength, _shakeDuration, token: _juiceCts.Token)
                .SuppressCancellationThrow()
                .Forget();
        }

        public void ShowSuccessBurst()
        {
            if (UIVfxLayer.Instance != null)
                UIVfxLayer.Instance.BurstAt(_shakeTarget, 18);

            CancelJuice();
            _juiceCts = CancellationTokenSource.CreateLinkedTokenSource(this.GetCancellationTokenOnDestroy());

            UITween.Punch(_shakeTarget, 0.2f, 0.34f, _juiceCts.Token)
                .SuppressCancellationThrow()
                .Forget();
        }

        private void CancelJuice()
        {
            _juiceCts?.Cancel();
            _juiceCts?.Dispose();
            _juiceCts = null;
        }
    }
}
