using UI.Services;
using Zenject;

namespace Core.Services.Hints
{
    /// <summary>
    /// Points the player at the opening cluster of a word they have not solved yet, and at a
    /// free row to drop it into. Deliberately does not place anything automatically — the
    /// player keeps agency, and the placement pipeline stays driven purely by pointer input.
    /// </summary>
    public sealed class HintService : IHintService
    {
        [Inject] private readonly IWordProgressService _progressService;
        [Inject] private readonly IClustersService _clustersService;
        [Inject] private readonly IWordContainersService _containersService;

        public bool TryShowHint()
        {
            var unsolved = _progressService.GetUnsolvedWords();

            if (unsolved.Count == 0)
                return false;

            for (var wordIndex = 0; wordIndex < unsolved.Count; wordIndex++)
            {
                var clusters = unsolved[wordIndex].clusters;

                if (clusters == null || clusters.Length == 0)
                    continue;

                var openingCluster = FindOpeningCluster(clusters);

                if (openingCluster == null)
                    continue;

                var spawned = _clustersService.SpawnedClusters;

                for (var i = 0; i < spawned.Count; i++)
                {
                    var view = spawned[i];

                    if (view == null || view.Presenter == null)
                        continue;

                    // Cluster views hold the very ClusterData instances from the level, so
                    // reference identity is enough — no value matching needed.
                    if (!ReferenceEquals(view.Presenter.GetData(), openingCluster))
                        continue;

                    view.PlayHintPulse();
                    PulseFirstFreeContainer();

                    return true;
                }
            }

            return false;
        }

        private static DataBase.Models.ClusterData FindOpeningCluster(DataBase.Models.ClusterData[] clusters)
        {
            for (var i = 0; i < clusters.Length; i++)
            {
                if (clusters[i].orderInWord == 0)
                    return clusters[i];
            }

            return clusters[0];
        }

        private void PulseFirstFreeContainer()
        {
            var containers = _containersService.ContainerPresenters;

            if (containers == null)
                return;

            for (var i = 0; i < containers.Length; i++)
            {
                if (containers[i].IsFullyFilled)
                    continue;

                containers[i].PlayHintPulse();
                return;
            }
        }
    }
}
