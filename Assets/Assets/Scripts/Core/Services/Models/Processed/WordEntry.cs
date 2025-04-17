using System;

namespace Core.Services.Models
{
    [Serializable]
    public class WordEntry
    {
        public string word;
        public ClusterData[] clusters;
    }
}