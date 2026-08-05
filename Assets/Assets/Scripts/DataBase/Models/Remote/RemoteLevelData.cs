using System;

namespace DataBase.Models
{
    [Serializable]
    public class RemoteLevelData
    {
        public int id;
        public string category;
        public string[] words;
    }
}