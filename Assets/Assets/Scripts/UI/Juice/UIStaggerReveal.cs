using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Juice
{
    public enum EStaggerDirection
    {
        FromBelow = 0,
        FromAbove = 1,
        ScaleOnly = 2
    }

    /// <summary>
    /// Reveals direct children one after another. Attach to a layout group
    /// (word grid, victory word list, cluster panel) and call <see cref="Play"/>,
    /// or leave <see cref="_playOnEnable"/> on for fire-and-forget screens.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class UIStaggerReveal : MonoBehaviour
    {
        [SerializeField] private bool _playOnEnable = true;
        [SerializeField] private EStaggerDirection _direction = EStaggerDirection.FromBelow;
        [SerializeField] private float _stepDelay = 0.05f;
        [SerializeField] private float _itemDuration = 0.34f;
        [SerializeField] private float _offset = 48f;
        [SerializeField] private float _fromScale = 0.8f;

        private CancellationTokenSource _cts;

        private void OnEnable()
        {
            if (_playOnEnable)
                Play();
        }

        private void OnDisable() => Cancel();

        private void OnDestroy() => Cancel();

        public void Play()
        {
            Cancel();

            // Children are spawned right before this call, so settle the layout first —
            // otherwise the reveal would read positions the layout group has not assigned yet.
            if (transform is RectTransform rect)
                LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
            _cts = CancellationTokenSource.CreateLinkedTokenSource(this.GetCancellationTokenOnDestroy());

            RunAsync(_cts.Token).SuppressCancellationThrow().Forget();
        }

        private async UniTask RunAsync(CancellationToken token)
        {
            var count = transform.childCount;

            for (var i = 0; i < count; i++)
            {
                var child = transform.GetChild(i) as RectTransform;

                if (child == null || !child.gameObject.activeSelf)
                    continue;

                RevealAsync(child, i * _stepDelay, token).SuppressCancellationThrow().Forget();
            }

            await UniTask.CompletedTask;
        }

        private async UniTask RevealAsync(RectTransform child, float delay, CancellationToken token)
        {
            var group = child.GetComponent<CanvasGroup>();

            if (group == null)
                group = child.gameObject.AddComponent<CanvasGroup>();

            var baseScale = child.localScale;
            var basePosition = child.anchoredPosition;

            group.alpha = 0f;

            switch (_direction)
            {
                case EStaggerDirection.FromBelow:
                    child.anchoredPosition = basePosition + new Vector2(0f, -_offset);
                    break;
                case EStaggerDirection.FromAbove:
                    child.anchoredPosition = basePosition + new Vector2(0f, _offset);
                    break;
                case EStaggerDirection.ScaleOnly:
                    child.localScale = baseScale * _fromScale;
                    break;
            }

            if (delay > 0f)
                await UniTask.Delay((int)(delay * 1000f), DelayType.UnscaledDeltaTime, cancellationToken: token);

            if (child == null || group == null)
                return;

            var scaleOnly = _direction == EStaggerDirection.ScaleOnly;

            var moveBack = scaleOnly
                ? UITween.Scale(child, baseScale, _itemDuration, EEase.OutBack, token)
                : UITween.AnchoredMove(child, basePosition, _itemDuration, EEase.OutBack, token);

            await UniTask.WhenAll(
                UITween.Fade(group, 1f, _itemDuration * 0.7f, EEase.OutQuad, token),
                moveBack);

            if (child == null || group == null)
                return;

            // In ScaleOnly mode the position belongs to the layout group. Writing a value
            // captured before the first layout pass would collapse the whole grid.
            if (!scaleOnly)
                child.anchoredPosition = basePosition;

            child.localScale = baseScale;
            group.alpha = 1f;
        }

        private void Cancel()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
        }
    }
}
