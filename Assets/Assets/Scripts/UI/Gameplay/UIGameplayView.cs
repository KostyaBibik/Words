using System.Collections.Generic;
using Core.Services.Models;
using UI.Abstract;
using UnityEngine;

namespace UI.Gameplay
{
    public class UIGameplayView : UIView
    {
        [SerializeField] private UIClustersHandler _clustersHandler;
        [SerializeField] private UIWordContainersHandler _wordContainersHandler;
        
        public void SetClusters(List<ClusterData> clusters)
        {
            _clustersHandler.UpdateClusters(clusters);
        }
        
        public void SetupWordContainers(int wordCount)
        {
            _wordContainersHandler.Initialize(wordCount, lettersPerWord: 6); 
        }
    }
}