using System.Threading;
using Cysharp.Threading.Tasks;
using UI.Juice;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Gameplay
{
    /// <summary>
    /// Full-screen tint sitting behind the gameplay window's content, coloured per level
    /// category (see <see cref="CategoryTheme"/>). Implemented as a UI overlay rather than by
    /// touching the 3D Grid/Skybox materials directly, so the theming stays isolated from the
    /// hand-tuned background scene.
    /// </summary>
    [RequireComponent(typeof(Image))]
    public sealed class UICategoryBackgroundView : MonoBehaviour
    {
        [SerializeField] private Image _tint;
        [SerializeField] private float _fadeDuration = 0.5f;

        private CancellationTokenSource _cts;

        private void Awake()
        {
            if (_tint == null)
                _tint = GetComponent<Image>();
        }

        private void OnDestroy()
        {
            _cts?.Cancel();
            _cts?.Dispose();
        }

        public void SetCategory(string category)
        {
            var color = CategoryTheme.Resolve(category);

            _cts?.Cancel();
            _cts?.Dispose();
            _cts = CancellationTokenSource.CreateLinkedTokenSource(this.GetCancellationTokenOnDestroy());

            UITween.TintGraphic(_tint, color, _fadeDuration, EEase.OutQuad, _cts.Token)
                .SuppressCancellationThrow()
                .Forget();
        }
    }
}
