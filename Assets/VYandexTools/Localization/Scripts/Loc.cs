using System;
using UnityEngine.Localization.Settings;

namespace VYandexTools.Localization.Scripts
{
    /// <summary>
    /// Synchronous localization facade for code-built/composed text. Boot initializes Unity
    /// Localization before loading the game scene, so table values are ready when UI is built.
    /// </summary>
    public static class Loc
    {
        internal const string TableName = "LocalizationTable";
        private const string EmptyLocaleCode = "em";

        public static bool IsEmptyLocale
        {
            get
            {
                try
                {
                    var locale = LocalizationSettings.SelectedLocale;
                    return locale != null &&
                           string.Equals(locale.Identifier.Code, EmptyLocaleCode, StringComparison.OrdinalIgnoreCase);
                }
                catch
                {
                    return false;
                }
            }
        }

        public static string Text(LocalizationKey key, string fallback = "")
        {
            try
            {
                if (!LocalizationSettings.HasSettings || LocalizationSettings.SelectedLocale == null)
                    return fallback;

                if (IsEmptyLocale)
                    return string.Empty;

                string value = LocalizationSettings.StringDatabase.GetLocalizedString(TableName, key.ToString());
                return string.IsNullOrEmpty(value) ? fallback : value;
            }
            catch
            {
                return fallback;
            }
        }

        public static string Format(LocalizationKey key, string fallback, params object[] values)
        {
            string format = Text(key, fallback);
            if (string.IsNullOrEmpty(format))
                return format;

            try
            {
                return string.Format(format, values);
            }
            catch (FormatException)
            {
                return fallback;
            }
        }
    }

    /// <summary>
    /// Stores an Editor-only locale choice between Play Mode sessions.
    /// Runtime builds never set this value.
    /// </summary>
    public static class LocalizationTestOverride
    {
        public const string PlayerPrefsKey = "Words.Localization.TestLocale";

        public static string LocaleCode
        {
            get => UnityEngine.PlayerPrefs.GetString(PlayerPrefsKey, string.Empty);
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    Clear();
                else
                    UnityEngine.PlayerPrefs.SetString(PlayerPrefsKey, value.Trim());

                UnityEngine.PlayerPrefs.Save();
            }
        }

        public static bool HasLocaleCode => !string.IsNullOrWhiteSpace(LocaleCode);

        public static void Clear()
        {
            UnityEngine.PlayerPrefs.DeleteKey(PlayerPrefsKey);
            UnityEngine.PlayerPrefs.Save();
        }

#if UNITY_EDITOR
        // Applies the override regardless of which scene Play Mode starts from,
        // since Boot (which normally sets the locale) only lives in the boot scene.
        [UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static async void ApplyOnPlayModeStart()
        {
            if (!HasLocaleCode)
                return;

            string code = LocaleCode;
            await LocalizationSettings.InitializationOperation.Task;

            var locale = LocalizationSettings.AvailableLocales.GetLocale(code);
            if (locale != null)
                LocalizationSettings.SelectedLocale = locale;
        }
#endif
    }
}
