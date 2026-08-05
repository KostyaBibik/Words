using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UI.Abstract;
using UI.Juice;
using UnityEngine;

namespace UI.Victory.Grid
{
    public sealed class UIFinallyWordView : UIView
    {
        [SerializeField] private TextMeshProUGUI _text;

        [Header("Reveal")]
        [SerializeField] private float _revealDuration = 0.36f;
        [SerializeField] private float _fromScale = 0.6f;

        private CancellationTokenSource _revealCts;

        public void UpdateText(string text) =>
            _text.text = text;

        /// <summary>Pops the word in after <paramref name="delay"/> seconds — used to stagger the victory list.</summary>
        public void PlayReveal(float delay)
        {
            CancelReveal();
            _revealCts = CancellationTokenSource.CreateLinkedTokenSource(this.GetCancellationTokenOnDestroy());

            RevealAsync(delay, _revealCts.Token).SuppressCancellationThrow().Forget();
        }

        private async UniTask RevealAsync(float delay, CancellationToken token)
        {
            var rect = (RectTransform)transform;
            var baseScale = rect.localScale;

            _canvasGroup.alpha = 0f;
            rect.localScale = baseScale * _fromScale;

            if (delay > 0f)
                await UniTask.Delay((int)(delay * 1000f), DelayType.UnscaledDeltaTime, cancellationToken: token);

            await UniTask.WhenAll(
                UITween.Fade(_canvasGroup, 1f, _revealDuration * 0.6f, EEase.OutQuad, token),
                UITween.Scale(rect, baseScale, _revealDuration, EEase.OutBack, token));

            rect.localScale = baseScale;
            _canvasGroup.alpha = 1f;
        }

        private void CancelReveal()
        {
            _revealCts?.Cancel();
            _revealCts?.Dispose();
            _revealCts = null;
        }

        public override void Dispose()
        {
            CancelReveal();
            base.Dispose();
        }
    }
}
