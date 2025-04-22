using System.Linq;
using Core.Factories;
using Core.Services.Models;
using Core.Systems.WordContainer;
using UI.Abstract;
using UI.Gameplay.WordContainers;
using Zenject;

namespace UI.Gameplay
{
    public class UIWordGridPresenter : UIPresenter<UIWordGridView>
    {
        private IUIWordContainerFactory _wordContainerFactory;
        private WordRepositoryTracker _tracker;
        private UIWordContainerPresenter[] _containerPresenters;

        public UIWordContainerPresenter[] ContainerPresenters => _containerPresenters;
        
        public UIWordGridPresenter(UIWordGridView view) : base(view)
        {
        }

        [Inject]
        public void Construct(
            IUIWordContainerFactory wordContainerFactory,
            WordRepositoryTracker wordRepositoryTracker
        )
        {
            _wordContainerFactory = wordContainerFactory;
            _tracker = wordRepositoryTracker;
        }

        public void UpdateData(int wordCount, int lettersPerWord)
        {
            var containerPrefab = _view.ContainerPrefab;
            var containerParent = _view.ContainersParent;
            
            _containerPresenters =
                _wordContainerFactory.CreateWordContainers(
                        containerPrefab,
                        containerParent,
                        lettersPerWord,
                        wordCount
                    );

            InitializeTrackingFilledStatus();
        }

        private void InitializeTrackingFilledStatus()
        {
            foreach (var container in _containerPresenters)
            {
                _tracker.Track(container, index => new ClusterData
                {
                    wordGroupIndex = index,
                    value = new string(container.GetPlacedClusters()
                        .OrderBy(kvp => kvp.Value)
                        .SelectMany(kvp => kvp.Key.GetData().value)
                        .ToArray())
                });
            }
        }
    }
}