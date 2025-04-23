using Core.Systems.Placeholder;
using DataBase.Models;
using UI.Factories;
using UI.Gameplay;
using UI.Gameplay.ClustersPanel;
using UI.Gameplay.Elements;
using UI.Services;
using UnityEngine;

namespace Core.Services
{
    public sealed class UIClustersService : IClustersService
    {
        private ClusterDragCoordinator _dragCoordinator;
        private UIPlaceholderView _placeholder;
        private ClusterPlaceholderHandler _placeholderHandler;
        private ClusterSpawner _clusterSpawner;
        private ClusterDragObserver _dragObserver;
        private UIClusterElementView[] _spawnedClusters;

        private readonly IUIClusterFactory _clusterFactory;

        public UIClustersService(IUIClusterFactory clusterFactory)
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
                Object.Destroy(_spawnedClusters[i].gameObject); 
            }
            
            Object.Destroy(_placeholder.gameObject); 
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