using Core.Factories;
using Core.Services.Models;
using Core.Systems.Placeholder;
using UI.Gameplay;
using UI.Gameplay.BottomPanel;
using UI.Gameplay.Elements;
using UnityEngine;

namespace Core.Services
{
    public class UIClustersService : IClustersService
    {
        private ClusterDragCoordinator _dragCoordinator;
        private UIPlaceholderView _placeholder;
        private ClusterPlaceholderHandler _placeholderHandler;
        private ClusterSpawner _clusterSpawner;
        private ClusterDragObserver _dragObserver;
        
        private readonly IClusterFactory _clusterFactory;

        public UIClustersService(IClusterFactory clusterFactory)
        {
            _clusterFactory = clusterFactory;
        }

        public void Initialize(ClusterPanelSettings settings, Canvas canvas)
        {
            InitializeDragCoordinator(settings, canvas);
            InitializePlaceholderSystem(settings);
            InitializeSpawnerAndObserver(settings);
        }

        public void UpdateClusters(ClusterData[] clusters, MonoBehaviour owner)
        {
            var spawnedClusters = _clusterSpawner.SpawnClusters(clusters);
            
            for (var index = 0; index < spawnedClusters.Length; index++)
            {
                var cluster = spawnedClusters[index];
                _dragObserver.Observe(cluster, owner);
            }
        }

        private void InitializeDragCoordinator(ClusterPanelSettings settings, Canvas canvas)
        {
            _dragCoordinator = new ClusterDragCoordinator(settings.DragLayer, canvas);
        }

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