using System.Collections.Generic;
using DataBase.Models;
using Cysharp.Threading.Tasks;
using UI.Services;
using UniRx;
using Zenject;

namespace Core.Services.Validation
{
    public sealed class ValidationService : IValidationService
    {
        [Inject] private readonly IGameDataRepository _gameDataRepository;
        [Inject] private readonly IWordContainersService _containersService;
        
        private readonly ReactiveProperty<bool> _validationStatus = new(false);
        
        public IReadOnlyReactiveProperty<bool> ValidationStatus => _validationStatus;

        public async UniTask<bool> Validate()
        {
            var level = _gameDataRepository.CurrentLevel;
            var expectedWords = level.words;

            var placedClusters = new List<(ClusterData clusterData, int startIndex)>();
            
            foreach (var container in _containersService.ContainerPresenters)
            {
                var containerClusters = container.GetPlacedClusters();
                
                foreach (var (clusterPresenter, startIndex) in containerClusters)
                {
                    var cluster = (clusterPresenter.GetData(), startIndex);
                    placedClusters.Add(cluster);
                }
            }
            
            _validationStatus.Value = await AreAllClustersPlacedCorrectly(expectedWords, placedClusters);

            return _validationStatus.Value;
        }

        public void Clear() => _validationStatus.Value = false;

        private async UniTask<bool> AreAllClustersPlacedCorrectly(
            WordEntry[] expectedWords,
            List<(ClusterData clusterData, int startIndex)> actualClusters
        )
        {
            return await UniTask.RunOnThreadPool(() =>
            {
                var matchedWordIndices = new HashSet<int>();

                for (var actualClusterIndex = 0; actualClusterIndex < actualClusters.Count; actualClusterIndex++)
                {
                    var actualClusterData = actualClusters[actualClusterIndex].clusterData;

                    if (actualClusterData.orderInWord != 0)
                        continue;

                    for (var expectedWordIndex = 0; expectedWordIndex < expectedWords.Length; expectedWordIndex++)
                    {
                        if (matchedWordIndices.Contains(expectedWordIndex))
                            continue;

                        var expectedClusters = expectedWords[expectedWordIndex].clusters;

                        if (expectedClusters.Length == 0 || expectedClusters[0].value != actualClusterData.value)
                            continue;

                        var actualGroupIndex = actualClusterData.wordGroupIndex;

                        var matchingGroup = new List<ClusterData>();

                        for (var i = 0; i < actualClusters.Count; i++)
                        {
                            var candidate = actualClusters[i].clusterData;

                            if (candidate.wordGroupIndex == actualGroupIndex)
                                matchingGroup.Add(candidate);
                        }

                        if (matchingGroup.Count != expectedClusters.Length)
                            continue;

                        var isCorrect = true;

                        for (var clusterIterator = 0; clusterIterator < expectedClusters.Length; clusterIterator++)
                        {
                            var expectedCluster = expectedClusters[clusterIterator];

                            var matchFound = false;

                            for (var matchIterator = 0; matchIterator < matchingGroup.Count; matchIterator++)
                            {
                                var actualCluster = matchingGroup[matchIterator];

                                if (actualCluster.orderInWord == expectedCluster.orderInWord &&
                                    actualCluster.value == expectedCluster.value)
                                {
                                    matchFound = true;
                                    break;
                                }
                            }

                            if (!matchFound)
                            {
                                isCorrect = false;
                                break;
                            }
                        }

                        if (!isCorrect)
                            return false;

                        matchedWordIndices.Add(expectedWordIndex);
                        break;
                    }
                }

                return matchedWordIndices.Count == expectedWords.Length;
            });
        }
    }
}