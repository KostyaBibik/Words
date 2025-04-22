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
        
        public ProcessedLevelData[] Process(RemoteLevelData[] levels)
        {
            var processedLevels = new List<ProcessedLevelData>();
            
            for (var levelIterator = 0; levelIterator < levels.Length; levelIterator++)
            {
                var level = levels[levelIterator];
                
                if (CheckWordsLength(level.words))
                {
                    Debug.LogWarning($"Level {level.id} contains words that do not meet the length requirement ({REQUIRED_WORD_LENGTH} letters). Skipping level.");
                    continue;  
                }

                var processedWords = new WordEntry[level.words.Length];
                
                for (var wordIterator = 0; wordIterator < level.words.Length; wordIterator++)
                {
                    var word = level.words[wordIterator];
                    var clusters =  GenerateClusters(word, wordIterator); 
                    
                    processedWords[wordIterator] = new WordEntry
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

        private ClusterData[] GenerateClusters(string word, int wordIndex)
        {
            var clusterCount = Random.Range(2, 4);  
            var clusters = new ClusterData[clusterCount];
            var clusterLength = word.Length / clusterCount;

            for (var iterator = 0; iterator < clusterCount; iterator++)
            {
                var clusterValue = word.Substring(iterator * clusterLength, clusterLength);
                clusters[iterator] = new ClusterData
                {
                    value = clusterValue,
                    orderInWord = iterator,
                    wordGroupIndex = wordIndex
                };
            }

            return clusters;
        }
    }
}
