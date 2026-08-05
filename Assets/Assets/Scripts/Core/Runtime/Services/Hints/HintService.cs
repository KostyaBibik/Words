using Cysharp.Threading.Tasks;
using DataBase.Models;
using UI.Gameplay.Elements;
using UI.Gameplay.WordContainers;
using UI.Juice;
using UI.Services;
using UnityEngine;
using Zenject;

namespace Core.Services.Hints
{
    /// <summary>
    /// Points the player at the next cluster piece they need for some unsolved word, and at
    /// the exact slot range it belongs in. Deliberately does not place anything automatically —
    /// the player keeps agency, and the placement pipeline stays driven purely by pointer input.
    /// </summary>
    public sealed class HintService : IHintService
    {
        [Inject] private readonly IWordProgressService _progressService;
        [Inject] private readonly IClustersService _clustersService;
        [Inject] private readonly IWordContainersService _containersService;

        private struct Candidate
        {
            public WordEntry Word;
            public ClusterData Cluster;
            public UIClusterElementView View;
            public UIWordContainerPresenter Container;
            public int StartIndex;
            public int ProgressLength;
        }

        public bool TryShowHint()
        {
            var unsolved = _progressService.GetUnsolvedWords();

            if (unsolved.Count == 0)
                return false;

            var hasBest = false;
            Candidate best = default;

            for (var wordIndex = 0; wordIndex < unsolved.Count; wordIndex++)
            {
                var clusters = unsolved[wordIndex].clusters;

                if (clusters == null || clusters.Length == 0)
                    continue;

                var (cluster, view) = FindNextUnplacedCluster(clusters);

                if (cluster == null || view == null)
                    continue;

                var (container, startIndex, progressLength) = ResolveTarget(unsolved[wordIndex], cluster);

                var candidate = new Candidate
                {
                    Word = unsolved[wordIndex],
                    Cluster = cluster,
                    View = view,
                    Container = container,
                    StartIndex = startIndex,
                    ProgressLength = progressLength
                };

                if (!hasBest || IsBetter(candidate, best))
                {
                    best = candidate;
                    hasBest = true;
                }
            }

            if (!hasBest)
                return false;

            PlayHintAsync(best).Forget();

            return true;
        }

        /// <summary>
        /// A word that already has a matching, in-progress container beats a completely fresh
        /// one (finish what's started), and having ANY legitimate target beats having none —
        /// never prefer a candidate we'd have to fall back to pointing at nothing over one with
        /// a concrete destination.
        /// </summary>
        private static bool IsBetter(Candidate candidate, Candidate current)
        {
            var candidateHasTarget = candidate.Container != null;
            var currentHasTarget = current.Container != null;

            if (candidateHasTarget != currentHasTarget)
                return candidateHasTarget;

            return candidate.ProgressLength > current.ProgressLength;
        }

        private async UniTaskVoid PlayHintAsync(Candidate candidate)
        {
            if (!_clustersService.IsClusterVisible(candidate.View))
                await _clustersService.RevealCluster(candidate.View);

            candidate.View.PlayHintPulse();

            if (UIVfxLayer.Instance != null)
                UIVfxLayer.Instance.BurstAt((RectTransform)candidate.View.transform, 6, 320f);

            if (candidate.Container == null || candidate.StartIndex < 0)
                return;

            candidate.Container.PlayHintPulse();
            candidate.Container.PlayHintForRange(candidate.StartIndex, candidate.Cluster.value.Length);
        }

        /// <summary>Lowest-order cluster of the word that has never been successfully dropped anywhere.</summary>
        private (ClusterData cluster, UIClusterElementView view) FindNextUnplacedCluster(ClusterData[] clusters)
        {
            ClusterData bestCluster = null;
            UIClusterElementView bestView = null;

            for (var i = 0; i < clusters.Length; i++)
            {
                var view = FindClusterView(clusters[i]);

                // A cluster with a container reference has been dropped successfully at least
                // once; UIClusterElementPresenter only ever sets that reference on a valid drop.
                if (view == null || view.Presenter.GetContainer() != null)
                    continue;

                if (bestCluster == null || clusters[i].orderInWord < bestCluster.orderInWord)
                {
                    bestCluster = clusters[i];
                    bestView = view;
                }
            }

            return (bestCluster, bestView);
        }

        private UIClusterElementView FindClusterView(ClusterData data)
        {
            var spawned = _clustersService.SpawnedClusters;

            for (var i = 0; i < spawned.Count; i++)
            {
                var view = spawned[i];

                if (view == null || view.Presenter == null)
                    continue;

                // Cluster views hold the very ClusterData instances from the level, so
                // reference identity is enough — no value matching needed.
                if (ReferenceEquals(view.Presenter.GetData(), data))
                    return view;
            }

            return null;
        }

        /// <summary>
        /// Picks where the hinted cluster should go. Only two outcomes count as a legitimate
        /// target: a container that already holds a correct prefix of THIS word, or a container
        /// that is completely empty. A container holding pieces of some other, unrelated word
        /// is never suggested — dropping a piece there would just make things worse, and pointing
        /// at it is actively misleading. When no legitimate target exists, Container is null and
        /// the caller falls back to pulsing the pool cluster only, without a slot highlight.
        /// </summary>
        private (UIWordContainerPresenter container, int startIndex, int progressLength) ResolveTarget(
            WordEntry word, ClusterData cluster)
        {
            var containers = _containersService.ContainerPresenters;

            if (containers == null || containers.Length == 0)
                return (null, -1, 0);

            var expectedWord = Normalize(word.word);

            UIWordContainerPresenter bestContainer = null;
            var bestPrefixLength = -1;

            for (var i = 0; i < containers.Length; i++)
            {
                var container = containers[i];

                if (container.IsFullyFilled)
                    continue;

                var assembled = Normalize(container.GetAssembledWord());

                if (assembled.Length == 0)
                    continue;

                if (expectedWord.StartsWith(assembled) && assembled.Length > bestPrefixLength)
                {
                    bestContainer = container;
                    bestPrefixLength = assembled.Length;
                }
            }

            if (bestContainer != null)
                return (bestContainer, bestPrefixLength, bestPrefixLength);

            for (var i = 0; i < containers.Length; i++)
            {
                if (containers[i].GetPlacedClusters().Count == 0)
                    return (containers[i], 0, 0);
            }

            return (null, -1, 0);
        }

        private static string Normalize(string value) =>
            string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpperInvariant();
    }
}
