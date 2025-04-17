using System.Collections.Generic;
using Core.Services.Models;
using UnityEngine;

namespace UI.Gameplay
{
    public class UIClustersHandler : MonoBehaviour
    {
        [SerializeField] private Transform _clustersContainer;
        [SerializeField] private UIClusterElementView _cluster2LettersPrefab; 
        [SerializeField] private UIClusterElementView _cluster3LettersPrefab; 
        
        private List<UIClusterElementView> _spawnedClusters = new();

        public void UpdateView(List<ClusterData> clusters)
        {
            ClearClusters(); 

            foreach (var clusterData in clusters)
            {
                var prefab = clusterData.value.Length switch
                {
                    2 => _cluster2LettersPrefab,
                    3 => _cluster3LettersPrefab,
                    _ => throw new System.NotSupportedException($"Cluster length {clusterData.value.Length} is not supported!")
                };

                var clusterView = Instantiate(prefab, _clustersContainer);
                clusterView.Setup(clusterData);
                _spawnedClusters.Add(clusterView);
            }
        }

        private void ClearClusters()
        {
            foreach (var cluster in _spawnedClusters)
            {
                if (cluster != null)
                    Destroy(cluster.gameObject);
            }
            _spawnedClusters.Clear();
        }
    }
}