using UnityEngine;

namespace UI.Juice
{
    /// <summary>
    /// Slow sine drift so static screens (menu title, victory panel) never feel frozen.
    /// Pure Update math — no allocations, negligible cost on WebGL.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(RectTransform))]
    public sealed class UIIdleFloat : MonoBehaviour
    {
        [SerializeField] private float _verticalAmplitude = 8f;
        [SerializeField] private float _horizontalAmplitude;
        [SerializeField] private float _rotationAmplitude;
        [SerializeField] private float _speed = 1.1f;
        [Tooltip("Desynchronises identical elements so they do not bob in lockstep.")]
        [SerializeField] private float _phaseOffset;
        [SerializeField] private bool _randomizePhase = true;

        private RectTransform _rect;
        private Vector2 _basePosition;
        private float _phase;

        private void Awake()
        {
            _rect = (RectTransform)transform;
            _basePosition = _rect.anchoredPosition;
            _phase = _randomizePhase ? Random.Range(0f, Mathf.PI * 2f) : _phaseOffset;
        }

        private void OnDisable() => _rect.anchoredPosition = _basePosition;

        private void Update()
        {
            var t = Time.unscaledTime * _speed + _phase;

            _rect.anchoredPosition = _basePosition + new Vector2(
                Mathf.Sin(t * 0.7f) * _horizontalAmplitude,
                Mathf.Sin(t) * _verticalAmplitude);

            if (_rotationAmplitude > 0f)
                _rect.localEulerAngles = new Vector3(0f, 0f, Mathf.Sin(t * 0.85f) * _rotationAmplitude);
        }
    }
}
