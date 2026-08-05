using System;
using UI.Gameplay.Elements;
using UnityEngine;
using UnityEngine.UI;

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
        [Tooltip("Scrollable pool of clusters. Used by the hint system to check visibility and scroll a hinted cluster into view.")]
        [SerializeField] private ScrollRect _scrollRect;

        public UIClusterElementView ClusterPrefab => _clusterPrefab;
        public UILetterView LetterPrefab => _letterPrefab;
        public UIPlaceholderView PlaceholderPrefab => _placeholderPrefab;
        public Transform ClustersContainer => _clustersContainer;
        public Transform DragLayer => _dragLayer;
        public ScrollRect ScrollRect => _scrollRect;
    }
}