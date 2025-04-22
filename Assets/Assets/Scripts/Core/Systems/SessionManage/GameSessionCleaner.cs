using Core.Services;
using Core.Systems.WordContainer;

namespace Core.Systems.SessionManage
{
    public sealed class GameSessionCleaner : IGameSessionCleaner
    {
        private readonly IClustersService _clustersService;
        private readonly IWordContainersService _containersService;
        private readonly IValidationService _validationService;
        private readonly WordRepositoryTracker _wordRepositoryTracker;

        public GameSessionCleaner(
            IClustersService clustersService,
            IWordContainersService containersService,
            IValidationService validationService,
            WordRepositoryTracker wordRepositoryTracker)
        {
            _clustersService = clustersService;
            _containersService = containersService;
            _validationService = validationService;
            _wordRepositoryTracker = wordRepositoryTracker;
        }

        public void Cleanup()
        {
            _clustersService.Clear();
            _containersService.Clear();
            _validationService.Clear();
            _wordRepositoryTracker.Clear();
        }
    }
}