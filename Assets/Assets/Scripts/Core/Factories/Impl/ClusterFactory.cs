using Core.Services.Models;
using UI.Gameplay.BottomPanel;
using UI.Gameplay.Elements;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Factories
{
    public class ClusterFactory : IClusterFactory
    {
        public UIClusterElementView CreateCluster(ClusterData data, ClusterPanelSettings settings)
        {
            var cluster = Object.Instantiate(settings.ClusterPrefab, settings.ClustersContainer);

            for (var index = 0; index < data.value.Length; index++)
            {
                var character = data.value[index];
                var letter = Object.Instantiate(settings.LetterPrefab, cluster.transform);
                letter.Setup(character);
                cluster.AddLetter(letter);
            }

            var rectTransform = settings.ClustersContainer.GetComponent<RectTransform>();
            LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
            
            return cluster;
        }

        public UIPlaceholderView CreatePlaceholder(ClusterPanelSettings settings)
        {
            var placeholder = Object.Instantiate(settings.PlaceholderPrefab, settings.ClustersContainer);
            placeholder.Deactivate();
            return placeholder;
        }
    }
}