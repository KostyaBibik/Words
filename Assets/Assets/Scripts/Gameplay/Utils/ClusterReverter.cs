using UI.Gameplay.Elements;

namespace UI.Gameplay.Utils
{
    public class ClusterReverter
    {
        public static void ReturnToOriginal(UIClusterElementView cluster)
        {
            var presenter = cluster.Presenter;
            
            presenter.ReturnToOriginalPosition();
            presenter.GetContainer()?.Presenter.ReturnClusterToPosition(cluster);
        }
    }
}