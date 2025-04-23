using DataBase.Models;
using UI.Gameplay.ClustersPanel;
using UnityEngine;

namespace UI.Services
{
    public interface IClustersService
    {
        public void Initialize(ClusterPanelSettings settings, Canvas canvas);
        public void UpdateClusters(ClusterData[] clusters, MonoBehaviour owner);
        public void Clear();
    }
}