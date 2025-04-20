using UI.Gameplay.Elements;

namespace UI.Gameplay.Utils
{
    public class ClusterReverter
    {
        public static void ReturnToOriginal(UIClusterElementView cluster)
        {
            cluster.ReturnToOriginalPosition();
            cluster.Container?.ReturnClusterToPosition(cluster);
        }
    }
}