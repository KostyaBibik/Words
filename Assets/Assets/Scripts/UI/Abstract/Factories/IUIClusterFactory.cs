using DataBase.Models;
using UI.Gameplay.ClustersPanel;
using UI.Gameplay.Elements;

namespace UI.Factories
{
    public interface IUIClusterFactory
    {
        public UIClusterElementView CreateCluster(ClusterData data, ClusterPanelSettings settings);
        public UIPlaceholderView CreatePlaceholder(ClusterPanelSettings settings);
    }
}