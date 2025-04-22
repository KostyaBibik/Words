using System;
using UI.Gameplay.Elements;
using UnityEngine;

namespace UI.Gameplay.ClustersPanel
{
    [Serializable]
    public class ClusterPanelSettings
    {
        [SerializeField] private UIClusterElementView _clusterPrefab;
        [SerializeField] private UILetterView _letterPrefab;
        [SerializeField] private UIPlaceholderView _placeholderPrefab;
        [SerializeField] private Transform _clustersContainer;
        [SerializeField] private Transform _dragLayer;

        public UIClusterElementView ClusterPrefab => _clusterPrefab;
        public UILetterView LetterPrefab => _letterPrefab;
        public UIPlaceholderView PlaceholderPrefab => _placeholderPrefab;
        public Transform ClustersContainer => _clustersContainer;
        public Transform DragLayer => _dragLayer;
    }
}