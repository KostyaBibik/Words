using System;

namespace DataBase.Models
{
    [Serializable]
    public class ProcessedLevelData
    {
        public int id;
        public WordEntry[] words;
    }
}