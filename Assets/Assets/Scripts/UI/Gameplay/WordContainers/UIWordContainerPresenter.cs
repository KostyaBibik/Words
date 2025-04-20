using Core.Factories;
using Core.Systems.WordContainer;
using UI.Abstract;
using UI.Gameplay.Elements;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Gameplay.WordContainers
{
    public class UIWordContainerPresenter : UIPresenter<UIWordContainerView>
    {
        private readonly IWordContainerFactory _wordContainerFactory;

        private WordContainerData _dataModel;
        
        private WordSlotHandler _slotHandler;
        private ClusterPlacementHelper _placementHelper;
        private ClusterTracker _clusterTracker;
        private SlotPlaceholderHelper _placeholderHelper;
        
        public UIWordContainerPresenter(UIWordContainerView view, IWordContainerFactory wordContainerFactory) 
            : base(view)
        {
            _wordContainerFactory = wordContainerFactory;
            
            SubscribeToViewEvents();
        }
        
        public void InitializeContainer(int wordLength)
        {
            var letterSlots = _wordContainerFactory.CreateLetterSlots(_view.LetterSlotPrefab, _view.transform, wordLength);
            
            _dataModel = new WordContainerData(letterSlots);
            
            _slotHandler = new WordSlotHandler(_dataModel);
            _placeholderHelper = new SlotPlaceholderHelper(_dataModel);
            _placementHelper = new ClusterPlacementHelper(_dataModel, _slotHandler, _placeholderHelper);
            _clusterTracker = new ClusterTracker(_dataModel, _slotHandler);
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
        
        public void ShowPlaceholder(UIClusterElementView cluster, int startIndex) =>
            _placeholderHelper.ShowPlaceholder(cluster, startIndex);
        
        public void ClearPlaceholder() =>
            _placeholderHelper.ClearPlaceholder();

        public int CalculateSlotIndexFromPosition(Vector2 localPosition) => 
            _placementHelper.CalculateSlotIndexFromPosition(localPosition);

        public void ReturnClusterToPosition(UIClusterElementView cluster) =>
            _clusterTracker.ReturnCluster(cluster);

        public void ReleaseSlotsForCluster(UIClusterElementView cluster) =>
            _slotHandler.ReleaseSlots(cluster);

        public void ClearBuffers() =>
            _dataModel.BufferSlots.Clear();

        private void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag != null && 
                eventData.pointerDrag.TryGetComponent<UIClusterElementView>(out var cluster))
            {
                TryDrop(cluster, eventData);
            }
        }

        private bool TryDrop(UIClusterElementView cluster, PointerEventData eventData) =>
            _placementHelper.TryDropCluster(cluster, eventData, _view.transform);
    }
}