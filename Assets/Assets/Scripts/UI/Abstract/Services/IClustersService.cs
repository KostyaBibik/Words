using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DataBase.Models;
using UI.Gameplay.ClustersPanel;
using UI.Gameplay.Elements;
using UnityEngine;

namespace UI.Services
{
    public interface IClustersService
    {
        /// <summary>Clusters currently alive in the session, in spawn order. Empty before the first level.</summary>
        IReadOnlyList<UIClusterElementView> SpawnedClusters { get; }

        public void Initialize(ClusterPanelSettings settings, Canvas canvas);
        public void UpdateClusters(ClusterData[] clusters, MonoBehaviour owner);
        public void Clear();

        /// <summary>True when enough of the cluster is inside the scrollable pool's viewport to be seen.</summary>
        bool IsClusterVisible(UIClusterElementView cluster);

        /// <summary>Scrolls the pool so the cluster ends up centred in the viewport.</summary>
        UniTask RevealCluster(UIClusterElementView cluster);
    }
}