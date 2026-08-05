using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace UI.Juice
{
    /// <summary>
    /// Minimal allocation-light tween helpers built on UniTask.
    /// Deliberately avoids a third-party tween engine: WebGL builds pay for every extra dependency.
    /// All tweens run on unscaled time so UI stays alive while the game is paused.
    /// Every tween aborts silently once its target is destroyed — runtime-spawned clusters and
    /// word rows are torn down between levels while their animations may still be in flight.
    /// </summary>
    public static class UITween
    {
        /// <param name="owner">Tween target. When it is destroyed the tween stops instead of throwing.</param>
        public static async UniTask Value(float from, float to, float duration, EEase ease,
            Action<float> onUpdate, CancellationToken token = default, Object owner = null)
        {
            var tracked = owner != null;

            if (duration <= 0f)
            {
                if (!tracked || owner != null)
                    onUpdate(to);

                return;
            }

            var elapsed = 0f;

            while (elapsed < duration)
            {
                if (tracked && owner == null)
                    return;

                token.ThrowIfCancellationRequested();

                elapsed += Time.unscaledDeltaTime;
                var t = Ease.Evaluate(ease, elapsed / duration);
                onUpdate(Mathf.LerpUnclamped(from, to, t));

                await UniTask.Yield(PlayerLoopTiming.Update, token);
            }

            if (tracked && owner == null)
                return;

            onUpdate(to);
        }

        public static UniTask Scale(Transform target, Vector3 to, float duration, EEase ease,
            CancellationToken token = default)
        {
            if (target == null)
                return UniTask.CompletedTask;

            var from = target.localScale;

            return Value(0f, 1f, duration, ease,
                t => target.localScale = Vector3.LerpUnclamped(from, to, t), token, target);
        }

        public static UniTask ScaleUniform(Transform target, float to, float duration, EEase ease,
            CancellationToken token = default) =>
            Scale(target, Vector3.one * to, duration, ease, token);

        public static UniTask Fade(CanvasGroup group, float to, float duration, EEase ease,
            CancellationToken token = default)
        {
            if (group == null)
                return UniTask.CompletedTask;

            var from = group.alpha;

            return Value(0f, 1f, duration, ease,
                t => group.alpha = Mathf.LerpUnclamped(from, to, t), token, group);
        }

        public static UniTask AnchoredMove(RectTransform target, Vector2 to, float duration, EEase ease,
            CancellationToken token = default)
        {
            if (target == null)
                return UniTask.CompletedTask;

            var from = target.anchoredPosition;

            return Value(0f, 1f, duration, ease,
                t => target.anchoredPosition = Vector2.LerpUnclamped(from, to, t), token, target);
        }

        public static UniTask WorldMove(Transform target, Vector3 to, float duration, EEase ease,
            CancellationToken token = default)
        {
            if (target == null)
                return UniTask.CompletedTask;

            var from = target.position;

            return Value(0f, 1f, duration, ease,
                t => target.position = Vector3.LerpUnclamped(from, to, t), token, target);
        }

        public static UniTask LocalMove(Transform target, Vector3 to, float duration, EEase ease,
            CancellationToken token = default)
        {
            if (target == null)
                return UniTask.CompletedTask;

            var from = target.localPosition;

            return Value(0f, 1f, duration, ease,
                t => target.localPosition = Vector3.LerpUnclamped(from, to, t), token, target);
        }

        public static UniTask RotateZ(Transform target, float to, float duration, EEase ease,
            CancellationToken token = default)
        {
            if (target == null)
                return UniTask.CompletedTask;

            var from = target.localEulerAngles.z;

            if (from > 180f)
                from -= 360f;

            return Value(from, to, duration, ease,
                z => target.localEulerAngles = new Vector3(0f, 0f, z), token, target);
        }

        public static UniTask TintGraphic(Graphic graphic, Color to, float duration, EEase ease,
            CancellationToken token = default)
        {
            if (graphic == null)
                return UniTask.CompletedTask;

            var from = graphic.color;

            return Value(0f, 1f, duration, ease,
                t => graphic.color = Color.LerpUnclamped(from, to, t), token, graphic);
        }

        public static UniTask TintText(TMP_Text text, Color to, float duration, EEase ease,
            CancellationToken token = default)
        {
            if (text == null)
                return UniTask.CompletedTask;

            var from = text.color;

            return Value(0f, 1f, duration, ease,
                t => text.color = Color.LerpUnclamped(from, to, t), token, text);
        }

        /// <summary>Scale up past the base value then settle back — the classic "pop" acknowledgement.</summary>
        public static async UniTask Punch(Transform target, float strength = 0.18f, float duration = 0.28f,
            CancellationToken token = default)
        {
            if (target == null)
                return;

            var baseScale = target.localScale;
            var peak = baseScale * (1f + strength);

            await Scale(target, peak, duration * 0.35f, EEase.OutQuad, token);
            await Scale(target, baseScale, duration * 0.65f, EEase.OutBack, token);
        }

        /// <summary>Horizontal damped shake used for "wrong answer" feedback.</summary>
        public static async UniTask Shake(RectTransform target, float strength = 24f, float duration = 0.4f,
            int oscillations = 6, CancellationToken token = default)
        {
            if (target == null)
                return;

            var origin = target.anchoredPosition;
            var elapsed = 0f;

            try
            {
                while (elapsed < duration)
                {
                    if (target == null)
                        return;

                    token.ThrowIfCancellationRequested();

                    elapsed += Time.unscaledDeltaTime;
                    var progress = elapsed / duration;
                    var damping = 1f - progress;
                    var offset = Mathf.Sin(progress * oscillations * Mathf.PI * 2f) * strength * damping;

                    target.anchoredPosition = origin + new Vector2(offset, 0f);

                    await UniTask.Yield(PlayerLoopTiming.Update, token);
                }
            }
            finally
            {
                if (target != null)
                    target.anchoredPosition = origin;
            }
        }

        /// <summary>Quick tilt-and-return, used when a cluster is grabbed.</summary>
        public static async UniTask Wobble(Transform target, float angle = 5f, float duration = 0.5f,
            CancellationToken token = default)
        {
            if (target == null)
                return;

            await RotateZ(target, angle, duration * 0.25f, EEase.OutQuad, token);
            await RotateZ(target, -angle * 0.6f, duration * 0.3f, EEase.InOutQuad, token);
            await RotateZ(target, 0f, duration * 0.45f, EEase.OutBack, token);
        }
    }
}
