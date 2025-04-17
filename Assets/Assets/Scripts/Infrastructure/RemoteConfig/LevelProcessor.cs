using System.Collections.Generic;
using System.Linq;
using Core.Services.Abstract;
using Core.Services.Models;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Infrastructure
{
    public class LevelProcessor : ILevelProcessor
    {
        private const int REQUIRED_WORD_LENGTH = 6;
        
        public async UniTask<ProcessedLevelData[]> Process(RemoteLevelData[] levels)
        {
            var processedLevels = new List<ProcessedLevelData>();
            
            for (int i = 0; i < levels.Length; i++)
            {
                var level = levels[i];
                
                if (CheckWordsLength(level.words))
                {
                    Debug.LogWarning($"Level {level.id} contains words that do not meet the length requirement ({REQUIRED_WORD_LENGTH} letters). Skipping level.");
                    continue;  
                }

                var processedWords = new WordEntry[level.words.Length];
                
                for (int j = 0; j < level.words.Length; j++)
                {
                    var word = level.words[j];
                    var clusters = await GenerateClusters(word); // Генерация кластеров асинхронно
                    
                    processedWords[j] = new WordEntry
                    {
                        word = word,
                        clusters = clusters
                    };
                }

                processedLevels.Add(new ProcessedLevelData
                {
                    id = level.id,
                    words = processedWords
                });
            }
            
            if (processedLevels.Count == 0)
            {
                Debug.LogError("No valid levels were processed.");
                return null; 
            }

            return processedLevels.ToArray();
        }

        private bool CheckWordsLength(string[] words)
        {
            return words.Any(word => word.Length != REQUIRED_WORD_LENGTH);
        }

        private async UniTask<ClusterData[]> GenerateClusters(string word)
        {
            int clusterCount = Random.Range(2, 4);  
            var clusters = new ClusterData[clusterCount];
            int clusterLength = word.Length / clusterCount;
            int index = 0;

            for (int i = 0; i < clusterCount; i++)
            {
                var clusterValue = word.Substring(i * clusterLength, clusterLength);
                clusters[i] = new ClusterData
                {
                    value = clusterValue,
                    orderInWord = i,
                    sourceWordIndex = index++
                };
            }

            return clusters;
        }
    }
}
