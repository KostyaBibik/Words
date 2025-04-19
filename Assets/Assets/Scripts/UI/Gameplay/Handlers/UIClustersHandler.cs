using System.Collections.Generic;
using Core.Services.Models;
using UI.Gameplay.Elements;
using UnityEngine;

namespace UI.Gameplay
{
    public class UIClustersHandler : MonoBehaviour
    {
        [System.Serializable]
        public class Settings
        {
            public UIClusterElementView clusterPrefab;
            public UILetterView letterPrefab;
            public UIPlaceholderView placeholderPrefab;
            public Transform clustersContainer;
            public Transform dragLayer;
        }

        [SerializeField] private Settings _settings;

        private List<UIClusterElementView> _clusters = new();
        private ClusterDragController _dragController;
        private UIPlaceholderView _placeholder;

        private void Awake()
        {
            var canvas = GetComponentInParent<Canvas>();
            _dragController = new ClusterDragController( _settings.dragLayer, canvas); 

            _placeholder = Instantiate(_settings.placeholderPrefab, _settings.clustersContainer);
            _placeholder.Deactivate();
        }

        public void UpdateClusters(List<ClusterData> clusters)
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
            var cluster = Instantiate(_settings.clusterPrefab, _settings.clustersContainer);
            
            foreach (char c in data.value)
            {
                var letter = Instantiate(_settings.letterPrefab, cluster.transform);
                letter.Setup(c);
                cluster.AddLetter(letter);
            }

            return cluster;
        }

        private void SetupClusterDragHandlers(UIClusterElementView cluster)
        {
            cluster.OnDragStarted += view =>
            {
                var index = cluster.transform.GetSiblingIndex();
                if(cluster.Container == null)
                {
                    _placeholder.Activate(cluster.GetComponent<RectTransform>());
                    _placeholder.transform.SetSiblingIndex(index);
                }
                
                _dragController.HandleBeginDrag(cluster);
            };

            cluster.OnDragging += position =>
            {
                _dragController.HandleDrag(position);
            };

            cluster.OnDragEnded += eventData =>
            {
                _placeholder.Deactivate();
                _dragController.HandleEndDrag(eventData);
            };
        }

        private void ClearClusters()
        {
            foreach (var cluster in _clusters)
            {
                if (cluster != null)
                    Destroy(cluster.gameObject);
            }
            _clusters.Clear();
        }
    }
}
