using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Juice
{
    /// <summary>
    /// Screen-space particle bursts drawn as pooled UI images.
    /// A ParticleSystem cannot draw above a Screen Space Overlay canvas without a second
    /// camera+canvas, so UI quads are used instead: one draw call, no camera changes, WebGL friendly.
    /// Place on a full-screen, non-raycastable RectTransform that is the last sibling of the UI root.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class UIVfxLayer : MonoBehaviour
    {
        private const int DEFAULT_POOL_SIZE = 48;

        public static UIVfxLayer Instance { get; private set; }

        [SerializeField] private Sprite _particleSprite;
        [SerializeField] private int _poolSize = DEFAULT_POOL_SIZE;
        [SerializeField] private Vector2 _particleSize = new(22f, 22f);
        [SerializeField] private float _gravity = 2600f;
        [SerializeField] private Color[] _palette =
        {
            new(1f, 0.85f, 0.25f),
            new(1f, 0.45f, 0.2f),
            new(0.3f, 0.85f, 1f),
            new(1f, 1f, 1f),
            new(0.55f, 1f, 0.55f)
        };

        private readonly Stack<Image> _pool = new();
        private RectTransform _rect;
        private CancellationTokenSource _cts;

        private void Awake()
        {
            Instance = this;
            _rect = (RectTransform)transform;
            _cts = CancellationTokenSource.CreateLinkedTokenSource(this.GetCancellationTokenOnDestroy());

            WarmPool();
        }

        private void OnDestroy()
        {
            if (Instance == this)
                Instance = null;

            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
        }

        private void WarmPool()
        {
            for (var i = 0; i < _poolSize; i++)
                _pool.Push(CreateParticle());
        }

        private Image CreateParticle()
        {
            var go = new GameObject("VfxParticle", typeof(RectTransform), typeof(Image));
            go.transform.SetParent(_rect, false);

            var image = go.GetComponent<Image>();
            image.sprite = _particleSprite;
            image.raycastTarget = false;

            var rect = (RectTransform)go.transform;
            rect.sizeDelta = _particleSize;

            go.SetActive(false);

            return image;
        }

        /// <summary>Radial burst of confetti at a world position (use a RectTransform's position).</summary>
        public void BurstAtWorld(Vector3 worldPosition, int count = 14, float force = 900f)
        {
            if (_cts == null)
                return;

            for (var i = 0; i < count; i++)
            {
                var angle = Random.Range(0f, Mathf.PI * 2f);
                var speed = force * Random.Range(0.45f, 1f);
                var velocity = new Vector2(Mathf.Cos(angle), Mathf.Abs(Mathf.Sin(angle)) + 0.35f) * speed;

                AnimateParticle(worldPosition, velocity, _cts.Token).SuppressCancellationThrow().Forget();
            }
        }

        public void BurstAt(RectTransform target, int count = 14, float force = 900f)
        {
            if (target != null)
                BurstAtWorld(target.position, count, force);
        }

        /// <summary>Confetti falling from above the screen — used on the victory screen.</summary>
        public void Rain(int count = 40, float force = 260f)
        {
            if (_cts == null)
                return;

            RainAsync(count, force, _cts.Token).SuppressCancellationThrow().Forget();
        }

        private async UniTask RainAsync(int count, float force, CancellationToken token)
        {
            var corners = new Vector3[4];
            _rect.GetWorldCorners(corners);

            var left = corners[0].x;
            var right = corners[2].x;
            var top = corners[1].y;

            for (var i = 0; i < count; i++)
            {
                var spawn = new Vector3(Random.Range(left, right), top + Random.Range(0f, 200f), 0f);
                var velocity = new Vector2(Random.Range(-force, force), Random.Range(-force * 0.2f, force * 0.4f));

                AnimateParticle(spawn, velocity, token).SuppressCancellationThrow().Forget();

                await UniTask.Delay(30, DelayType.UnscaledDeltaTime, cancellationToken: token);
            }
        }

        private async UniTask AnimateParticle(Vector3 worldPosition, Vector2 velocity, CancellationToken token)
        {
            var image = _pool.Count > 0 ? _pool.Pop() : CreateParticle();
            var rect = (RectTransform)image.transform;

            image.color = _palette.Length > 0 ? _palette[Random.Range(0, _palette.Length)] : Color.white;
            image.gameObject.SetActive(true);

            rect.position = worldPosition;
            rect.localScale = Vector3.one * Random.Range(0.7f, 1.25f);

            var spin = Random.Range(-540f, 540f);
            var lifetime = Random.Range(0.75f, 1.3f);
            var elapsed = 0f;
            var baseColor = image.color;

            try
            {
                while (elapsed < lifetime)
                {
                    if (rect == null || image == null)
                        return;

                    token.ThrowIfCancellationRequested();

                    var dt = Time.unscaledDeltaTime;
                    elapsed += dt;

                    velocity.y -= _gravity * dt;
                    rect.position += (Vector3)(velocity * dt);
                    rect.Rotate(0f, 0f, spin * dt);

                    var fade = 1f - Mathf.Clamp01(elapsed / lifetime);
                    image.color = new Color(baseColor.r, baseColor.g, baseColor.b, fade);

                    await UniTask.Yield(PlayerLoopTiming.Update, token);
                }
            }
            finally
            {
                if (image != null)
                {
                    image.gameObject.SetActive(false);
                    _pool.Push(image);
                }
            }
        }
    }
}
