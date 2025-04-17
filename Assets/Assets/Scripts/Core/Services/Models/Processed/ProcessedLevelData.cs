using System;

namespace Core.Services.Models
{
    [Serializable]
    public class ProcessedLevelData
    {
        public int id;
        public WordEntry[] words;
    }
}