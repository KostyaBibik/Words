using System.Collections.Generic;
using Core.Services.Models;
using Core.Systems.Clusters;
using UI.Gameplay.Elements;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Gameplay
{
    public class UIClustersHandler : MonoBehaviour
    {
        [System.Serializable]
        public class Settings
        {
            public UIClusterElementView cluster2LettersPrefab;
            public UIClusterElementView cluster3LettersPrefab;
            public UIPlaceholderView placeholderPrefab;
            public Transform clustersContainer;
            public Transform dragLayer;
        }

        [SerializeField] private Settings _settings;
    
        private readonly List<UIClusterElementView> _clusters = new();
        private ClusterDragController _dragController;
        private UIPlaceholderView _placeholder;

        private void Awake()
        {
            var canvas = GetComponentInParent<Canvas>();
            _dragController = new ClusterDragController(canvas, _settings.dragLayer);
            _placeholder = Instantiate(_settings.placeholderPrefab, _settings.clustersContainer);
            _placeholder.Deactivate();
        }

        public void UpdateView(List<ClusterData> clusters)
        {
            ClearClusters();
            foreach (var data in clusters)
            {
                var cluster = CreateCluster(data);
                SetupClusterDragHandlers(cluster);
                _clusters.Add(cluster);
            }
        }
        
        private UIClusterElementView CreateCluster(ClusterData data)
        {
            var prefab = data.value.Length == 2 
                ? _settings.cluster2LettersPrefab 
                : _settings.cluster3LettersPrefab;
            
            var cluster = Instantiate(prefab, _settings.clustersContainer);
            cluster.Setup(data.value);
            return cluster;
        }

        private void SetupClusterDragHandlers(UIClusterElementView cluster)
        {
            cluster.OnDragStarted += eventData =>
            {
                var index = cluster.transform.GetSiblingIndex();
                _placeholder.Activate(cluster.GetComponent<RectTransform>());
                _placeholder.transform.SetSiblingIndex(index);
                _dragController.HandleBeginDrag(cluster);
            };
        
            cluster.OnDragging += _dragController.HandleDrag;
        
            cluster.OnDragEnded += eventData =>
            {
                _placeholder.Deactivate();
                _dragController.HandleEndDrag(WasDroppedInValidZone(eventData));
            };
        }
        
        private bool WasDroppedInValidZone(PointerEventData eventData)
        {
            return false;
        }

        private void ClearClusters()
        {
            foreach (var cluster in _clusters)
            {
                if (cluster != null) Destroy(cluster.gameObject);
            }
            _clusters.Clear();
        }
    }
}