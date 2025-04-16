using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.RemoteConfig;
using Unity.Services.Core;
using UnityEngine;

public class RemoteConfigTest : MonoBehaviour
{
     [System.Serializable]
    public class LevelData
    {
        public int id;
        public string[] words;
        public string[] clusters;
    }

    [System.Serializable]
    public class LevelsContainer
    {
        public LevelData[] levels;
    }

    private struct UserAttributes { }
    private struct AppAttributes { }

    private async void Start()
    {
        // Инициализация сервисов
        await InitializeRemoteConfig();

        // Загрузка конфигурации
        await FetchConfig();

        string idTest = RemoteConfigService.Instance.appConfig.GetInt("test_id").ToString();
        Debug.Log($"Raw JSON from Remote Config: {idTest}");

        // Получение и парсинг JSON
        string json = RemoteConfigService.Instance.appConfig.GetJson("levels_json");
        Debug.Log($"Raw JSON from Remote Config: {json}");

        if (!string.IsNullOrEmpty(json))
        {
            try
            {
                LevelsContainer data = JsonUtility.FromJson<LevelsContainer>(json);
                Debug.Log($"Successfully parsed {data.levels.Length} levels");
                Debug.Log($"First word: {data.levels[0].words[0]}");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"JSON parse error: {e.Message}");
            }
        }
        else
        {
            Debug.LogError("Received empty JSON from Remote Config");
        }
    }

    private async Task InitializeRemoteConfig()
    {
        if (UnityServices.State != ServicesInitializationState.Initialized)
        {
            await UnityServices.InitializeAsync();
        }

        // Анонимная аутентификация (необязательно для Remote Config)
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
    }

    private async Task FetchConfig()
    {
        try
        {
            await RemoteConfigService.Instance.FetchConfigsAsync(
                new UserAttributes(), 
                new AppAttributes());
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Remote Config fetch failed: {e.Message}");
        }
    }
}