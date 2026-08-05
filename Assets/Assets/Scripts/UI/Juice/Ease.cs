using UnityEngine;

namespace UI.Juice
{
    public enum EEase
    {
        Linear,
        InQuad,
        OutQuad,
        InOutQuad,
        OutCubic,
        InBack,
        OutBack,
        OutElastic,
        OutBounce,
        OutExpo
    }

    public static class Ease
    {
        private const float BACK_C1 = 1.70158f;
        private const float BACK_C3 = BACK_C1 + 1f;
        private const float ELASTIC_C4 = 2f * Mathf.PI / 3f;

        public static float Evaluate(EEase ease, float t)
        {
            t = Mathf.Clamp01(t);

            switch (ease)
            {
                case EEase.InQuad:
                    return t * t;
                case EEase.OutQuad:
                    return 1f - (1f - t) * (1f - t);
                case EEase.InOutQuad:
                    return t < 0.5f
                        ? 2f * t * t
                        : 1f - Mathf.Pow(-2f * t + 2f, 2f) / 2f;
                case EEase.OutCubic:
                    return 1f - Mathf.Pow(1f - t, 3f);
                case EEase.InBack:
                    return BACK_C3 * t * t * t - BACK_C1 * t * t;
                case EEase.OutBack:
                    return 1f + BACK_C3 * Mathf.Pow(t - 1f, 3f) + BACK_C1 * Mathf.Pow(t - 1f, 2f);
                case EEase.OutElastic:
                    if (t <= 0f || t >= 1f)
                        return t;
                    return Mathf.Pow(2f, -10f * t) * Mathf.Sin((t * 10f - 0.75f) * ELASTIC_C4) + 1f;
                case EEase.OutBounce:
                    return OutBounce(t);
                case EEase.OutExpo:
                    return t >= 1f ? 1f : 1f - Mathf.Pow(2f, -10f * t);
                default:
                    return t;
            }
        }

        private static float OutBounce(float t)
        {
            const float n1 = 7.5625f;
            const float d1 = 2.75f;

            if (t < 1f / d1)
                return n1 * t * t;

            if (t < 2f / d1)
            {
                t -= 1.5f / d1;
                return n1 * t * t + 0.75f;
            }

            if (t < 2.5f / d1)
            {
                t -= 2.25f / d1;
                return n1 * t * t + 0.9375f;
            }

            t -= 2.625f / d1;
            return n1 * t * t + 0.984375f;
        }
    }
}
