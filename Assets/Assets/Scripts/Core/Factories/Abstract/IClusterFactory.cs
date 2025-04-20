using Core.Services.Models;
using UI.Gameplay.BottomPanel;
using UI.Gameplay.Elements;

namespace Core.Factories
{
    public interface IClusterFactory
    {
        public UIClusterElementView CreateCluster(ClusterData data, ClusterPanelSettings settings);
        public UIPlaceholderView CreatePlaceholder(ClusterPanelSettings settings);
    }
}