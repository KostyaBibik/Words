using System;

namespace DataBase.Models
{
    [Serializable]
    public class ProcessedLevelData
    {
        public int id;
        public string category;
        public WordEntry[] words;
    }
}