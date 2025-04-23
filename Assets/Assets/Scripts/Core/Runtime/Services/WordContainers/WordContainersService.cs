using UI.Factories;
using UI.Gameplay.Elements;
using UI.Gameplay.WordContainers;
using UI.Services;
using UnityEngine;

namespace Core.Services.WordContainers
{
    public sealed class WordContainersService : IWordContainersService
    {
        private UIWordContainerPresenter[] _containerPresenters;

        private readonly IUIWordContainerFactory _containerFactory;
        private readonly IWordRepositoryTracker _repositoryTracker;

        public UIWordContainerPresenter[] ContainerPresenters => _containerPresenters;
        
        public WordContainersService(IUIWordContainerFactory containerFactory, IWordRepositoryTracker repositoryTracker)
        {
            _containerFactory = containerFactory;
            _repositoryTracker = repositoryTracker;
        }

        public void UpdateClusters(UIWordContainerView prefab, Transform parent, int wordCount, int lettersPerWord)
        {
            _containerPresenters = _containerFactory
                .CreateWordContainers(
                    prefab,
                    parent,
                    lettersPerWord,
                    wordCount
                );
            
            InitializeTrackingFilledStatus();
        }

        public void Clear()
        {
            for (var iterator = 0; iterator < _containerPresenters.Length; iterator++)
            {
                _containerPresenters[iterator].Destroy();
            }
        }

        private void InitializeTrackingFilledStatus()
        {
            for (var iterator = 0; iterator < _containerPresenters.Length; iterator++)
            {
                var container = _containerPresenters[iterator];
                _repositoryTracker.Track(container);
            }
        }
    }
}