using Core.Services.Models;
using UI.Gameplay.ClustersPanel;
using UnityEngine;

namespace Core.Services
{
    public interface IClustersService
    {
        public void Initialize(ClusterPanelSettings settings, Canvas canvas);
        public void UpdateClusters(ClusterData[] clusters, MonoBehaviour owner);
        public void Clear();
    }
}