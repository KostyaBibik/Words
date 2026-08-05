using System;
using UnityEngine;

namespace Yandex
{
    /// <summary>
    /// Кроме проверки NoAds/доступности, гарантирует требование модерации 1.19.3 / 4.7: на время
    /// полноэкранной рекламы игра и звук ставятся на паузу, вызывается Gameplay.Stop(), а по
    /// закрытию — всё возвращается и Gameplay.Start().
    /// </summary>
    public static class Advertisement
    {
        private static bool _adActive;
        private static float _savedTimeScale = 1f;

        private static void OnAdOpen(Action onOpen)
        {
            if (!_adActive)
            {
                _adActive = true;
                _savedTimeScale = Time.timeScale;
                Time.timeScale = 0f;
                AudioListener.pause = true;
                Gameplay.Stop();
            }
            onOpen?.Invoke();
        }

        private static void OnAdClose(Action onClose)
        {
            if (_adActive)
            {
                _adActive = false;
                Time.timeScale = _savedTimeScale;
                AudioListener.pause = false;
                Gameplay.Start();
            }
            onClose?.Invoke();
        }

        public static void ShowInterstitial(Action onOpenCallback = null, Action onCloseCallback = null,
            Action<string> onErrorCallback = null, Action onOfflineCallback = null, string placement = "")
        {
            // Guard against calls before Boot finished SDK init: a button may fire an ad before
            // Advertisement.Initialize() ran. Kimicu's ShowInterstitialAd THROWS
            // "Advertisement not initialized!" in that window, which halts WebGL.
            if (SaveSystem.SaveData.NoAds
                || !Kimicu.YandexGames.Advertisement.Initialized
                || !Kimicu.YandexGames.Advertisement.AdvertisementIsAvailable)
            {
                onCloseCallback?.Invoke();
                return;
            }

            Kimicu.YandexGames.Advertisement.ShowInterstitialAd(
                () =>
                {
                    OnAdOpen(onOpenCallback);
                    GaEventProvider.InterstitialAdEvent(placement);
                },
                () => OnAdClose(onCloseCallback),
                error =>
                {
                    OnAdClose(null);
                    onErrorCallback?.Invoke(error);
                },
                onOfflineCallback);
        }

        public static void ShowReward(Action onOpenCallback = null, Action onRewardedCallback = null,
            Action onCloseCallback = null, Action<string> onErrorCallback = null, string placement = "")
        {
            // Same pre-init guard: ShowVideoAd throws "Advertisement not initialized!" before Init.
            if (!Kimicu.YandexGames.Advertisement.Initialized)
            {
                onCloseCallback?.Invoke();
                return;
            }

            Kimicu.YandexGames.Advertisement.ShowVideoAd(
                () => OnAdOpen(onOpenCallback),
                () =>
                {
                    onRewardedCallback?.Invoke();
                    GaEventProvider.RewardAdEvent(placement);
                },
                () => OnAdClose(onCloseCallback),
                error =>
                {
                    OnAdClose(null);
                    onErrorCallback?.Invoke(error);
                });
        }
    }
}