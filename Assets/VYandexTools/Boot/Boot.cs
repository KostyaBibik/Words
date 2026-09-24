using System.Collections;
using Agava.YandexGames;
using GameAnalyticsSDK;
using Kimicu.YandexGames;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using VYandexTools.Localization.Scripts;
using Billing = Kimicu.YandexGames.Billing;
using WebApplication = Kimicu.YandexGames.WebApplication;
using YandexGamesSdk = Kimicu.YandexGames.YandexGamesSdk;

namespace DefaultNamespace.Yandex
{
    public class Boot : MonoBehaviour
    {
        private const int GameSceneBuildIndex = 1;
        private const float GameAnalyticsInitializationTimeoutSeconds = 5f;
        private const float BillingTimeoutSeconds = 5f;

#if UNITY_EDITOR
        [SerializeField] private string locale = "ru";

#endif
        private bool _billingSuccses;
        private BootLoadingScreen _loadingScreen;

        private IEnumerator Start()
        {
            _loadingScreen = BootLoadingScreen.Show();

            yield return YandexGamesSdk.Initialize();
            _loadingScreen?.SetProgress(0.15f);

            yield return Cloud.Initialize();
            _loadingScreen?.SetProgress(0.35f);

            Advertisement.Initialize();
            WebApplication.Initialize(OnStopGame);
            _loadingScreen?.SetProgress(0.4f);

            yield return InitializeGameAnalytics();
            _loadingScreen?.SetProgress(0.5f);

            yield return Billing.Initialize();
            _loadingScreen?.SetProgress(0.7f);

            yield return Consume();
            _loadingScreen?.SetProgress(0.8f);

            SaveSystem.Instance.Init();
            yield return LocalizationSettings.InitializationOperation;
            SetLanguage();
            yield return LocalizationSettings.StringDatabase.GetTableAsync(Loc.TableName);
            _loadingScreen?.SetProgress(0.85f);

            global::Yandex.Advertisement.ShowInterstitial(placement: "boot");
            global::Yandex.Gameplay.Start();
            LoadScene();
        }

        // Never block startup on GameAnalytics: with unfilled keys in Settings.asset the SDK skips
        // GA_Wrapper.Initialize entirely, so IsRemoteConfigsReady() stays false forever and an
        // unbounded WaitUntil would hang the loading screen for good.
        private static IEnumerator InitializeGameAnalytics()
        {
            GameAnalytics.Initialize();

#if !UNITY_EDITOR
            float deadline = Time.realtimeSinceStartup + GameAnalyticsInitializationTimeoutSeconds;
            while (!GameAnalytics.IsRemoteConfigsReady() && Time.realtimeSinceStartup < deadline)
                yield return null;

            if (!GameAnalytics.IsRemoteConfigsReady())
                Debug.LogWarning("GameAnalytics remote config is unavailable. Continuing with local defaults.");
#endif
            yield break;
        }

        private IEnumerator Consume()
        {
            // Kimicu only answers in WebGL (or Editor with WebGL target); on error or any other
            // platform the callback never fires, so don't let an unbounded wait hang the boot.
            bool billingFailed = false;
            Billing.GetPurchasedProducts(UpdateProductCatalog, error =>
            {
                Debug.LogWarning($"GetPurchasedProducts failed: {error}");
                billingFailed = true;
            });

            float deadline = Time.realtimeSinceStartup + BillingTimeoutSeconds;
            while (!_billingSuccses && !billingFailed && Time.realtimeSinceStartup < deadline)
                yield return null;
        }

        private void UpdateProductCatalog(GetPurchasedProductsResponse response)
        {
            _billingSuccses = true;
            PurchasedProduct[] purchaseProducts = response.purchasedProducts;

            var countProducts = purchaseProducts.Length;
            for (var i = 0; i < countProducts; i++)
            {
                var product = purchaseProducts[i];
                if (product.productID.Equals(PurchaseIndexes.NoAD.ToString()))
                {
                    SaveSystem.SaveData.NoAds = true;
                    SaveSystem.Instance.SaveToStorage();
                }


                Billing.ConsumeProduct(product.purchaseToken);
            }
        }

        private void SetLanguage()
        {
#if UNITY_EDITOR
            string lang = LocalizationTestOverride.HasLocaleCode ? LocalizationTestOverride.LocaleCode : locale;
#else
             string lang = YandexGamesSdk.Environment.i18n.lang;
#endif
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.GetLocale(lang);
        }

        private static void OnStopGame(bool value)
        {
            if (value) global::Yandex.Gameplay.Start();
            else global::Yandex.Gameplay.Stop();

            AudioListener.volume = value ? 1 : 0;
            AudioListener.pause = !value;
            Time.timeScale = value ? 1 : 0;
        }

        private void LoadScene()
        {
            if (_loadingScreen != null)
                // Start on _loadingScreen, not on Boot: Boot lives in the Boot scene and gets
                // destroyed when it unloads, which would kill this coroutine mid-flight before
                // it reaches Destroy(_root). _loadingScreen is DontDestroyOnLoad'd and survives.
                _loadingScreen.StartCoroutine(_loadingScreen.LoadTargetScene(GameSceneBuildIndex, progressFrom: 0.85f));
            else
                SceneManager.LoadScene(GameSceneBuildIndex);
        }


        internal enum PurchaseIndexes
        {
            NoAD
        }
    }
}
