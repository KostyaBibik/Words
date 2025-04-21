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
                {
                    allPlaced.Add((clusterPresenter.GetData(), startIndex));
                }
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
                for (var wordIterator = 0; wordIterator < expectedWords.Length; wordIterator++)
                {
                    var word = expectedWords[wordIterator];
                    var clusters = word.clusters;

                    for (var clusterIterator = 0; clusterIterator < clusters.Length; clusterIterator++)
                    {
                        var expected = clusters[clusterIterator];
                        var matchFound = false;

                        for (var actualClusterIterator = 0; actualClusterIterator < actualClusters.Count; actualClusterIterator++)
                        {
                            var actual = actualClusters[actualClusterIterator].clusterData;

                            if (actual.value != expected.value ||
                                actual.orderInWord != expected.orderInWord ||
                                actual.sourceWordIndex != expected.sourceWordIndex)
                                continue;
                            
                            matchFound = true;
                            break;
                        }

                        if (!matchFound)
                            return false;
                    }
                }

                return true;
            });
        }
    }
}