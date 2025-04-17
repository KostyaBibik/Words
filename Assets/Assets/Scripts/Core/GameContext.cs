using System;
using Core.Services.Abstract;
using Cysharp.Threading.Tasks;
using UI.Flow;
using Zenject;

public class GameContext : IInitializable, IDisposable
{
    private readonly ILevelLoader _levelLoader;
    private readonly IUIFlowManager _uiFlow;

    [Inject]
    public GameContext(ILevelLoader levelLoader, IUIFlowManager uiFlow)
    {
        _levelLoader = levelLoader;
        _uiFlow = uiFlow;
    }

    public async void Initialize()
    {
        await LoadGameData();
    }

    private async UniTask LoadGameData()
    {
        _uiFlow.ShowLoadingScreen();
        
        try
        {
            _uiFlow.ShowMainMenuScreen();
        }
        catch (Exception e)
        {
            _uiFlow.ShowErrorScreen($"Ошибка загрузки: {e.Message}");
        }
    }

    public void Dispose() { }
}