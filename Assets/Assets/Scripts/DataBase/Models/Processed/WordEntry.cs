using System;

namespace DataBase.Models
{
    [Serializable]
    public class WordEntry
    {
        public string word;
        public ClusterData[] clusters;
    }
}