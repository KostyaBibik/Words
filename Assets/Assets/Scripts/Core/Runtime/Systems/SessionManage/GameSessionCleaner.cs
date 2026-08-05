using Core.Services;
using UI.Services;
using Zenject;

namespace Core.Systems.SessionManage
{
    public sealed class GameSessionCleaner : IGameSessionCleaner
    {
        [Inject] private readonly IClustersService _clustersService;
        [Inject] private readonly IWordContainersService _containersService;
        [Inject] private readonly IValidationService _validationService;
        [Inject] private readonly IWordRepositoryTracker _wordRepositoryTracker;
        [Inject] private readonly IWordProgressService _progressService;

        public void Cleanup()
        {
            // Drop progress subscriptions before the containers they point at are destroyed.
            _progressService.Clear();

            _clustersService.Clear();
            _containersService.Clear();
            _validationService.Clear();
            _wordRepositoryTracker.Clear();
        }
    }
}