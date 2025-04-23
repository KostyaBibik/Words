using DataBase.Models;
using UI.Factories;
using UI.Gameplay.ClustersPanel;
using UI.Gameplay.Elements;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Factories
{
    public class UIClusterFactory : IUIClusterFactory
    {
        public UIClusterElementView CreateCluster(ClusterData data, ClusterPanelSettings settings)
        {
            var clusterView = Object.Instantiate(settings.ClusterPrefab, settings.ClustersContainer);
            var clusterPresenter = new UIClusterElementPresenter(clusterView);

            clusterView.Initialize(clusterPresenter);
            clusterPresenter.Initialize();
            clusterPresenter.SetData(data);
            
            for (var index = 0; index < data.value.Length; index++)
            {
                var character = data.value[index];
                var letter = Object.Instantiate(settings.LetterPrefab, clusterView.transform);
                
                letter.Setup(character);
                clusterView.Presenter.AddLetter(letter);
            }

            clusterView.UpdateFrame();
            
            var rectTransform = settings.ClustersContainer.GetComponent<RectTransform>();
            LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
            
            return clusterView;
        }

        public UIPlaceholderView CreatePlaceholder(ClusterPanelSettings settings)
        {
            var placeholder = Object.Instantiate(settings.PlaceholderPrefab, settings.ClustersContainer);
            placeholder.Deactivate();
            return placeholder;
        }
    }
}