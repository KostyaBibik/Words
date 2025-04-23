using System.Linq;
using DataBase.Models;
using UI.Factories;
using UI.Gameplay.ClustersPanel;
using UI.Gameplay.Elements;
using UnityEngine;

namespace UI.Gameplay
{
    public sealed class ClusterSpawner
    {
        private readonly IUIClusterFactory _factory;
        private readonly ClusterPanelSettings _settings;

        public ClusterSpawner(IUIClusterFactory factory, ClusterPanelSettings settings)
        {
            _factory = factory;
            _settings = settings;
        }

        public UIClusterElementView[] SpawnClusters(ClusterData[] data)
        {
            var shuffledData = data.OrderBy(x => Random.value).ToArray();
            var clusters = new UIClusterElementView[data.Length];
            
            for (var iterator = 0; iterator < shuffledData.Length; iterator++)
            {
                var clusterData = shuffledData[iterator];
                var cluster = _factory.CreateCluster(clusterData, _settings);
                
                clusters[iterator] = cluster;
            }

            return clusters;
        }
    }
}