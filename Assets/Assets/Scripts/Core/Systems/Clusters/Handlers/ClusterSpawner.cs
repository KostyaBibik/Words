using Core.Factories;
using Core.Services.Models;
using UI.Gameplay.BottomPanel;
using UI.Gameplay.Elements;

namespace UI.Gameplay
{
    public class ClusterSpawner
    {
        private readonly IClusterFactory _factory;
        private readonly ClusterPanelSettings _settings;

        public ClusterSpawner(IClusterFactory factory, ClusterPanelSettings settings)
        {
            _factory = factory;
            _settings = settings;
        }

        public UIClusterElementView[] SpawnClusters(ClusterData[] data)
        {
            var clusters = new UIClusterElementView[data.Length];
            
            for (var iterator = 0; iterator < data.Length; iterator++)
            {
                var clusterData = data[iterator];
                var cluster = _factory.CreateCluster(clusterData, _settings);
                
                clusters[iterator] = cluster;
            }

            return clusters;
        }
    }
}