using System.Collections.Generic;
using System.Linq;
using Core.Services.Abstract;
using DataBase.Models;
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

        private bool CheckWordsLength(string[] words) =>
             words.Any(word => word.Length != REQUIRED_WORD_LENGTH);
        
        private ClusterData[] GenerateClusters(string word, int wordIndex)
        {
            int[] possibleSizes = { 2, 3, 4 };
    
            for (var attempts = 0; attempts < 100; attempts++)
            {
                var clusterSizes = new List<int>();
                var remainingLength = word.Length;
        
                while (remainingLength > 0)
                {
                    var availableSizes = possibleSizes.Where(s => s <= remainingLength).ToArray();
                    if (availableSizes.Length == 0)
                        break;
            
                    var chosenSize = availableSizes[Random.Range(0, availableSizes.Length)];
                    clusterSizes.Add(chosenSize);
                    remainingLength -= chosenSize;
                }
        
                if (remainingLength == 0 && clusterSizes.Count >= 2)
                {
                    var clusters = new ClusterData[clusterSizes.Count];
                    var position = 0;
            
                    for (var i = 0; i < clusterSizes.Count; i++)
                    {
                        var size = clusterSizes[i];
                        clusters[i] = new ClusterData
                        {
                            value = word.Substring(position, size),
                            orderInWord = i,
                            wordGroupIndex = wordIndex
                        };
                        position += size;
                    }
            
                    return clusters;
                }
            }
    
            return new[]
            {
                new ClusterData { value = word.Substring(0, 3), orderInWord = 0, wordGroupIndex = wordIndex },
                new ClusterData { value = word.Substring(3, 3), orderInWord = 1, wordGroupIndex = wordIndex },
            };
        }
    }
}
