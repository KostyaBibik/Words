using System;
using System.Collections.Generic;
using Kimicu.YandexGames;
using Newtonsoft.Json;
using UnityEngine;

public class SaveSystem : Singleton<SaveSystem>
{
    private static bool IsDataLoaded { get; set; }

    private static PlayerSaveData cachedSaveData;

    public static ref PlayerSaveData SaveData
    {
        get
        {
            if (!IsDataLoaded)
                cachedSaveData = LoadPlayerData();

            return ref cachedSaveData;
        }
    }

    private static string lastSavedJson;

    private const int SavingPeriod = 4;

    public override void Init()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        InvokeRepeating(nameof(SavePlayerData), SavingPeriod, SavingPeriod);
        DontDestroyOnLoad(this);
        base.Init();
    }

    private static bool _warnedCloudNotReady;

    private static PlayerSaveData LoadPlayerData()
    {
        // Guard: SaveData accessed before Cloud.Initialize() (gameplay scene entered without Boot,
        // or an init race). Return in-memory defaults WITHOUT caching (IsDataLoaded stays false) so
        // the real cloud data loads on the next access once Cloud is ready — instead of hard-crashing.
        if (!Cloud.Initialized)
        {
            if (!_warnedCloudNotReady)
            {
                Debug.LogWarning("SaveSystem: Cloud not initialized yet — returning defaults. " +
                                 "Boot must run first (scene 0).");
                _warnedCloudNotReady = true;
            }
            return new PlayerSaveData { Ints = new Dictionary<string, int>() };
        }

        string json = Cloud.GetValue("SaveData", "");

        PlayerSaveData saveData;

        if (!string.IsNullOrEmpty(json) && !string.IsNullOrWhiteSpace(json))
            saveData = JsonConvert.DeserializeObject<PlayerSaveData>(json);
        else
            saveData = new PlayerSaveData
            {
                Money = 0,
                NoAds = false,
            };

        // Null-checks for collections (NewtonsoftJson creates nulls when json has no info about them)
        saveData.Ints ??= new Dictionary<string, int>();
        saveData.Floats ??= new Dictionary<string, float>();
        saveData.Strings ??= new Dictionary<string, string>();

        IsDataLoaded = true;
        return saveData;
    }

    /// <summary>
    /// Полный сброс прогресса — для тестирования (см. <see cref="SaveResetCheat"/>).
    /// Чистит облако И кэш в памяти. Сброс кэша обязателен: без него ближайший автосейв через
    /// SavingPeriod секунд просто зальёт старые данные обратно — ровно поэтому ручная очистка
    /// облака в консоли Яндекса выглядит как «ничего не произошло».
    /// </summary>
    public static void ResetAllData()
    {
        cachedSaveData = new PlayerSaveData
        {
            Ints = new Dictionary<string, int>(),
            Floats = new Dictionary<string, float>(),
            Strings = new Dictionary<string, string>(),
        };
        IsDataLoaded = true;
        lastSavedJson = null;

        if (!Cloud.Initialized)
        {
            Debug.LogWarning("SaveSystem: cloud is not initialized — only in-memory data was reset.");
            return;
        }

        Cloud.SetValue("SaveData", JsonConvert.SerializeObject(cachedSaveData), true,
            () => Debug.Log("SaveSystem: cloud data was reset."));
    }

    public void SaveToStorage()
    {
        SavePlayerData();
    }

    private void SavePlayerData()
    {
        string json = JsonConvert.SerializeObject(cachedSaveData);

        if (json == lastSavedJson)
            return;
        Cloud.SetValue("SaveData", json, true, () => lastSavedJson = json);
    }
}

[Serializable]
public struct PlayerSaveData
{
    // Fill content of your SaveData, it can be anything that Newtonsoft can serialize
    // Example of reactive data:
    private int _cachedMoney;
    public static event Action OnMoneyChanged;

    public int Money
    {
        get => _cachedMoney;
        set
        {
            _cachedMoney = value;
            OnMoneyChanged?.Invoke();
        }
    }

    private bool _noAds;
    public static event Action OnBuyNoAds;

    public bool NoAds
    {
        get => _noAds;
        set
        {
            _noAds = value;
            OnBuyNoAds?.Invoke();
        }
    }

    private int _levelProgress;
    public static event Action OnLevelProgressChanged;

    public int LevelProgress
    {
        get => _levelProgress;
        set
        {
            _levelProgress = value;
            OnLevelProgressChanged?.Invoke();
        }
    }

    // Generic key-value stores for ported PlayerPrefs keys (see Prefs shim). Каждый произвольный
    // ключ игры живёт здесь, так что облачный сейв работает без правки call-site'ов.
    public Dictionary<string, int> Ints { get; set; }
    public Dictionary<string, float> Floats { get; set; }
    public Dictionary<string, string> Strings { get; set; }
}
