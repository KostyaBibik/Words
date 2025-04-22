using Core.Services.Models;
using UI.Gameplay.ClustersPanel;
using UI.Gameplay.Elements;

namespace Core.Factories
{
    public interface IUIClusterFactory
    {
        public UIClusterElementView CreateCluster(ClusterData data, ClusterPanelSettings settings);
        public UIPlaceholderView CreatePlaceholder(ClusterPanelSettings settings);
    }
}