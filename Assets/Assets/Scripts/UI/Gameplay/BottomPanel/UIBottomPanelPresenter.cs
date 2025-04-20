using Core.Services;
using Core.Services.Models;
using UI.Abstract;
using UnityEngine;
using Zenject;

namespace UI.Gameplay.BottomPanel
{
    public class UIBottomPanelPresenter : UIPresenter<UIBottomPanelView>
    {
        private IClustersService _clustersService;

        public UIBottomPanelPresenter(UIBottomPanelView view) : base(view)
        {
        }

        [Inject]
        public void Construct(IClustersService clustersService)
        {
            _clustersService = clustersService;
        }

        public override void Initialize()
        {
            var canvas = _view.transform.GetComponentInParent<Canvas>();
            _clustersService.Initialize(_view.ClusterPanelSettings, canvas);
        }

        public void UpdateData(ClusterData[] clusters) =>
            _clustersService.UpdateClusters(clusters, _view);
    }
}