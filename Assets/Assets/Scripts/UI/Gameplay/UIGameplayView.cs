using System.Collections.Generic;
using Core.Services.Models;
using UI.Abstract;
using UnityEngine;

namespace UI.Gameplay
{
    public class UIGameplayView : UIView
    {
        [SerializeField] private UIClustersHandler _clustersHandler;

        public void SetClusters(List<ClusterData> clusters)
        {
            _clustersHandler.UpdateView(clusters);
        }
    }
}