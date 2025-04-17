using System;

namespace Core.Services.Models
{
    [Serializable]
    public class ClusterData
    {
        public string value;
        public int orderInWord;
        public int sourceWordIndex;
    }
}