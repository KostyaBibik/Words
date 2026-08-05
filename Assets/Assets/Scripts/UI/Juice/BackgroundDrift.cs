using UnityEngine;

namespace UI.Juice
{
    /// <summary>
    /// Slowly scrolls and breathes the background grid so the board never looks like a static wallpaper.
    /// Operates on a material instance, so the shared asset is left untouched.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Renderer))]
    public sealed class BackgroundDrift : MonoBehaviour
    {
        private static readonly int MainTexId = Shader.PropertyToID("_MainTex");

        [SerializeField] private Vector2 _scrollSpeed = new(0.006f, 0.0025f);
        [Tooltip("Extra scale breathed in and out, as a fraction of the base scale. 0 disables it.")]
        [SerializeField] private float _breathAmount = 0.012f;
        [SerializeField] private float _breathSpeed = 0.35f;

        private Material _material;
        private Vector3 _baseScale;
        private Vector2 _baseOffset;
        private bool _hasMainTex;

        private void Awake()
        {
            _material = GetComponent<Renderer>().material;
            _hasMainTex = _material != null && _material.HasProperty(MainTexId);

            if (_hasMainTex)
                _baseOffset = _material.GetTextureOffset(MainTexId);

            _baseScale = transform.localScale;
        }

        private void OnDestroy()
        {
            if (_material != null)
                Destroy(_material);
        }

        private void Update()
        {
            var time = Time.unscaledTime;

            if (_hasMainTex)
                _material.SetTextureOffset(MainTexId, _baseOffset + _scrollSpeed * time);

            if (_breathAmount > 0f)
                transform.localScale = _baseScale * (1f + Mathf.Sin(time * _breathSpeed) * _breathAmount);
        }
    }
}
