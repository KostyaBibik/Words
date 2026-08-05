using Core.Systems.Placeholder;
using Cysharp.Threading.Tasks;
using DataBase.Models;
using UI.Factories;
using UI.Gameplay;
using UI.Gameplay.ClustersPanel;
using UI.Gameplay.Elements;
using UI.Juice;
using UI.Services;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Services
{
    public sealed class UIClustersService : IClustersService
    {
        private const float VisibleWidthThreshold = 0.6f;
        private const float RevealDuration = 0.35f;

        private ClusterDragCoordinator _dragCoordinator;
        private UIPlaceholderView _placeholder;
        private ClusterPlaceholderHandler _placeholderHandler;
        private ClusterSpawner _clusterSpawner;
        private ClusterDragObserver _dragObserver;
        private UIClusterElementView[] _spawnedClusters = System.Array.Empty<UIClusterElementView>();
        private ScrollRect _scrollRect;

        private readonly IUIClusterFactory _clusterFactory;

        public System.Collections.Generic.IReadOnlyList<UIClusterElementView> SpawnedClusters => _spawnedClusters;

        public UIClustersService(IUIClusterFactory clusterFactory)
        {
            _clusterFactory = clusterFactory;
        }

        public void Initialize(ClusterPanelSettings settings, Canvas canvas)
        {
            _scrollRect = settings.ScrollRect;

            InitializeDragCoordinator(settings, canvas);
            InitializePlaceholderSystem(settings);
            InitializeSpawnerAndObserver(settings);
        }

        public void UpdateClusters(ClusterData[] clusters, MonoBehaviour owner)
        {
            _spawnedClusters = _clusterSpawner.SpawnClusters(clusters);

            for (var index = 0; index < _spawnedClusters.Length; index++)
            {
                var cluster = _spawnedClusters[index];
                _dragObserver.Observe(cluster, owner);
            }
        }

        public void Clear()
        {
            for (var i = 0; i < _spawnedClusters.Length; i++)
            {
                if (_spawnedClusters[i] == null)
                    continue;

                // Unparent first: Destroy is deferred to the end of the frame and the next
                // level's clusters are spawned within it, so a still-parented cluster would
                // inflate the panel's layout while the new pool is being measured.
                _spawnedClusters[i].transform.SetParent(null, false);
                Object.Destroy(_spawnedClusters[i].gameObject);
            }

            _spawnedClusters = System.Array.Empty<UIClusterElementView>();

            if (_placeholder != null)
            {
                _placeholder.transform.SetParent(null, false);
                Object.Destroy(_placeholder.gameObject);
            }
        }

        public bool IsClusterVisible(UIClusterElementView cluster)
        {
            if (cluster == null || _scrollRect == null)
                return false;

            var viewport = _scrollRect.viewport != null ? _scrollRect.viewport : (RectTransform)_scrollRect.transform;
            var viewportRect = GetWorldRect(viewport);
            var targetRect = GetWorldRect((RectTransform)cluster.transform);

            if (targetRect.width <= 0f)
                return false;

            var overlapLeft = Mathf.Max(viewportRect.xMin, targetRect.xMin);
            var overlapRight = Mathf.Min(viewportRect.xMax, targetRect.xMax);
            var overlapWidth = Mathf.Max(0f, overlapRight - overlapLeft);

            return overlapWidth >= targetRect.width * VisibleWidthThreshold;
        }

        public async UniTask RevealCluster(UIClusterElementView cluster)
        {
            if (cluster == null || _scrollRect == null || _scrollRect.content == null)
                return;

            var content = _scrollRect.content;
            var viewport = _scrollRect.viewport != null ? _scrollRect.viewport : (RectTransform)_scrollRect.transform;
            var viewportTransform = _scrollRect.transform;

            Canvas.ForceUpdateCanvases();

            // Classic Unity ScrollRect "scroll to element" trick: express both the content's
            // current position and the target's position in the viewport's local frame. Their
            // difference IS the new content.anchoredPosition needed to land the target on the
            // viewport's origin (its pivot, i.e. the centre for a typically-pivoted viewport) —
            // it replaces the current position outright, it is not a delta to add to it.
            var targetLocal = (Vector2)viewportTransform.InverseTransformPoint(((RectTransform)cluster.transform).position);
            var contentLocal = (Vector2)viewportTransform.InverseTransformPoint(content.position);
            var newPosition = contentLocal - targetLocal;

            var maxScroll = Mathf.Max(0f, content.rect.width - viewport.rect.width);
            var targetX = Mathf.Clamp(newPosition.x, -maxScroll, 0f);

            var fromX = content.anchoredPosition.x;

            await UITween.Value(fromX, targetX, RevealDuration, EEase.OutCubic, x =>
            {
                var pos = content.anchoredPosition;
                pos.x = x;
                content.anchoredPosition = pos;
            });
        }

        private static Rect GetWorldRect(RectTransform rectTransform)
        {
            var corners = new Vector3[4];
            rectTransform.GetWorldCorners(corners);

            return new Rect(corners[0].x, corners[0].y, corners[2].x - corners[0].x, corners[2].y - corners[0].y);
        }

        private void InitializeDragCoordinator(ClusterPanelSettings settings, Canvas canvas) =>
            _dragCoordinator = new ClusterDragCoordinator(settings.DragLayer, canvas);

        private void InitializePlaceholderSystem(ClusterPanelSettings settings)
        {
            _placeholder = _clusterFactory.CreatePlaceholder(settings);
            _placeholderHandler = new ClusterPlaceholderHandler();
            _placeholderHandler.Initialize(_placeholder);
        }

        private void InitializeSpawnerAndObserver(ClusterPanelSettings settings)
        {
            _clusterSpawner = new ClusterSpawner(_clusterFactory, settings);
            _dragObserver = new ClusterDragObserver(_dragCoordinator, _placeholderHandler);
        }
    }
}
