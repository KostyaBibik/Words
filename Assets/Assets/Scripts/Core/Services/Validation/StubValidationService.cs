using System.Collections.Generic;
using Assets.Scripts.Core.Services.Abstract;
using Core.Services.Abstract;
using Core.Services.Models;
using Cysharp.Threading.Tasks;
using UI.Gameplay;
using UnityEngine;

namespace Core.Services.Validation
{
    public class StubValidationService : IValidationService
    {
        private readonly IGameDataRepository _gameDataRepository;
        private readonly UIWordGridPresenter _wordGridPresenter;

        public StubValidationService(IGameDataRepository gameDataRepository, UIWordGridPresenter wordGridPresenter)
        {
            _gameDataRepository = gameDataRepository;
            _wordGridPresenter = wordGridPresenter;
        }

        public async UniTask Validate()
        {
            var level = _gameDataRepository.CurrentLevel;
            var expectedWords = level.words;

            var allPlaced = new List<(ClusterData clusterData, int startIndex)>();

            foreach (var container in _wordGridPresenter.WordContainerPresenters)
            {
                var placed = container.GetPlacedClusters();
                foreach (var (clusterPresenter, startIndex) in placed)
                    allPlaced.Add((clusterPresenter.GetData(), startIndex));
            }

            var isAllPlaced = await AreAllClustersPlacedCorrectly(expectedWords, allPlaced);

            Debug.Log($"[Validation] isAllPlaced : {isAllPlaced}");

            await UniTask.CompletedTask;
        }

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

                        for (var i = 0; i < expectedClusters.Length; i++)
                        {
                            var expectedCluster = expectedClusters[i];

                            var matchFound = false;

                            for (var j = 0; j < matchingGroup.Count; j++)
                            {
                                var actualCluster = matchingGroup[j];

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