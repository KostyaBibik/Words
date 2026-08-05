#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Localization;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using VYandexTools.Localization.Scripts;

namespace VYandexTools.Localization.Editor
{
    public sealed class LocalizationTestLanguageWindow : EditorWindow
    {
        private const string MenuPath = "Tools/Localization/Test Language";
        private const string LocalesPath = "Assets/VYandexTools/Localization/Data/Locales";

        private readonly List<Locale> _locales = new List<Locale>();
        private string[] _options = System.Array.Empty<string>();
        private int _selectedIndex;

        [MenuItem(MenuPath)]
        private static void Open()
        {
            GetWindow<LocalizationTestLanguageWindow>("Test Language");
        }

        private void OnEnable()
        {
            RefreshLocales();
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Localization Test Language", EditorStyles.boldLabel);
            EditorGUILayout.Space(4f);

            if (!LocalizationSettings.HasSettings)
            {
                EditorGUILayout.HelpBox("LocalizationSettings asset is not configured.", MessageType.Warning);
                return;
            }

            if (_locales.Count == 0)
            {
                EditorGUILayout.HelpBox("No locale assets found. Check the localization setup.", MessageType.Warning);
                if (GUILayout.Button("Refresh"))
                    RefreshLocales();

                return;
            }

            _selectedIndex = Mathf.Clamp(_selectedIndex, 0, _locales.Count - 1);
            _selectedIndex = EditorGUILayout.Popup("Locale", _selectedIndex, _options);

            Locale currentLocale = LocalizationSettings.SelectedLocale;
            string currentCode = currentLocale == null ? "-" : currentLocale.Identifier.Code;
            string overrideCode = LocalizationTestOverride.HasLocaleCode
                ? LocalizationTestOverride.LocaleCode
                : "-";

            EditorGUILayout.LabelField("Current", currentCode);
            EditorGUILayout.LabelField("Editor Override", overrideCode);
            EditorGUILayout.Space(8f);

            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Apply"))
                    ApplySelectedLocale();

                if (GUILayout.Button("Clear Override"))
                    ClearOverride();

                if (GUILayout.Button("Refresh"))
                    RefreshLocales();
            }

            EditorGUILayout.Space(6f);
            EditorGUILayout.HelpBox(
                "Apply stores the selected locale for Editor Play Mode and switches the active locale immediately.",
                MessageType.Info);
        }

        private void RefreshLocales()
        {
            _locales.Clear();

            if (!LocalizationSettings.HasSettings)
            {
                _options = System.Array.Empty<string>();
                _selectedIndex = 0;
                return;
            }

            foreach (Locale locale in LocalizationEditorSettings.GetLocales())
                AddLocale(locale);

            if (_locales.Count == 0 && LocalizationSettings.AvailableLocales != null)
            {
                foreach (Locale locale in LocalizationSettings.AvailableLocales.Locales)
                    AddLocale(locale);
            }

            if (_locales.Count == 0)
            {
                foreach (string guid in AssetDatabase.FindAssets("t:Locale", new[] { LocalesPath }))
                {
                    string path = AssetDatabase.GUIDToAssetPath(guid);
                    AddLocale(AssetDatabase.LoadAssetAtPath<Locale>(path));
                }
            }

            _locales.Sort((left, right) =>
                string.Compare(left.Identifier.Code, right.Identifier.Code, System.StringComparison.Ordinal));

            _options = new string[_locales.Count];
            for (int i = 0; i < _locales.Count; i++)
            {
                Locale locale = _locales[i];
                string code = locale.Identifier.Code;
                string name = string.IsNullOrWhiteSpace(locale.LocaleName) ? locale.name : locale.LocaleName;
                _options[i] = $"{code} - {name}";
            }

            string selectedCode = LocalizationTestOverride.HasLocaleCode
                ? LocalizationTestOverride.LocaleCode
                : LocalizationSettings.SelectedLocale?.Identifier.Code;

            _selectedIndex = FindLocaleIndex(selectedCode);
            Repaint();
        }

        private void AddLocale(Locale locale)
        {
            if (locale == null)
                return;

            foreach (Locale existing in _locales)
            {
                if (existing.Identifier == locale.Identifier)
                    return;
            }

            _locales.Add(locale);
        }

        private void ApplySelectedLocale()
        {
            if (_locales.Count == 0)
                return;

            Locale selectedLocale = _locales[Mathf.Clamp(_selectedIndex, 0, _locales.Count - 1)];
            LocalizationTestOverride.LocaleCode = selectedLocale.Identifier.Code;
            LocalizationSettings.SelectedLocale = selectedLocale;
            Debug.Log($"[LocalizationTest] Locale override set to '{selectedLocale.Identifier.Code}'.");
            Repaint();
        }

        private void ClearOverride()
        {
            LocalizationTestOverride.Clear();
            Debug.Log("[LocalizationTest] Locale override cleared.");
            RefreshLocales();
        }

        private int FindLocaleIndex(string localeCode)
        {
            if (!string.IsNullOrWhiteSpace(localeCode))
            {
                for (int i = 0; i < _locales.Count; i++)
                {
                    if (_locales[i].Identifier.Code == localeCode)
                        return i;
                }
            }

            return 0;
        }
    }
}
#endif
