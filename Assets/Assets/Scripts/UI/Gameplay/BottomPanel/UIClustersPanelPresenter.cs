using Core.Services;
using Core.Services.Models;
using Core.Systems;
using UI.Abstract;
using UI.Gameplay.Elements;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace UI.Gameplay.ClustersPanel
{
    public class UIClustersPanelPresenter : UIPresenter<UIClustersPanelView>
    {
        private IClustersService _clustersService;
        private IDropPlacementHelper _dropPlacementHandler;

        public UIClustersPanelPresenter(UIClustersPanelView view) : base(view)
        {
            SubscribeToViewEvents();
        }

        [Inject]
        public void Construct(IClustersService clustersService)
        {
            _clustersService = clustersService;
        }

        public void UpdateData(ClusterData[] clusters)
        {
            InitializeServices();
            
            _clustersService.UpdateClusters(clusters, _view);
        }

        private void InitializeServices()
        {
            var settings = _view.ClusterPanelSettings;
            var canvas = _view.transform.GetComponentInParent<Canvas>();
            
            _clustersService.Initialize(settings, canvas);

            _dropPlacementHandler = new BottomDropPlacementHandler(settings.ClustersContainer);
        }

        private void SubscribeToViewEvents()
        {
            _view.OnClusterDropped
                .Subscribe(OnDrop)
                .AddTo(_view); 
            
            _view.OnTryDrop
                .Subscribe(tuple =>
                {
                    var (cluster, eventData, tcs) = tuple;
                    var result = TryDrop(cluster, eventData);
                    tcs.TrySetResult(result);
                })
                .AddTo(_view);
        }

        private void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag != null && 
                eventData.pointerDrag.TryGetComponent<UIClusterElementView>(out var cluster))
            {
                TryDrop(cluster, eventData);
            }
        }
        
        private bool TryDrop(UIClusterElementView cluster, PointerEventData eventData) =>
            _dropPlacementHandler.TryDropCluster(cluster, eventData);
    }
}