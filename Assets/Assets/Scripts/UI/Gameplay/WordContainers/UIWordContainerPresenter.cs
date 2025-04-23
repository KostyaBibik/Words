using System;
using System.Collections.Generic;
using UI.Abstract;
using UI.Factories;
using UI.Gameplay.Elements;
using UI.Models;
using UI.Services;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using Object = UnityEngine.Object;

namespace UI.Gameplay.WordContainers
{
    public sealed class UIWordContainerPresenter : UIPresenter<UIWordContainerView>
    {
        private readonly IUIWordContainerFactory _wordContainerFactory;
        private readonly IUIWordContainerDependenciesFactory _wordContainerDependenciesFactory;
        private readonly int _containerIndex;

        private IWordContainerModel _dataModel;
        
        private IWordSlotHandler _slotHandler;
        private IContainerDropPlacementHelper _dropPlacementHelper;
        private IClusterTracker _clusterTracker;
        private ISlotPlaceholderHelper _placeholderHelper;
        
        public int Index => _containerIndex;
        public IObservable<Unit> OnFullyFilled => _dataModel.OnFullyFilled;
        public IObservable<Unit> OnBecameIncomplete  => _dataModel.OnBecameIncomplete;
        public bool IsFullyFilled => _dataModel.IsFullyFilled.Value;

        private readonly CompositeDisposable _disposable = new();
        
        public UIWordContainerPresenter(
            UIWordContainerView view, 
            IUIWordContainerFactory wordContainerFactory,
            IUIWordContainerDependenciesFactory wordContainerDependenciesFactory,
            int containerIndex
        ) : base(view)
        {
            _wordContainerFactory = wordContainerFactory;
            _wordContainerDependenciesFactory = wordContainerDependenciesFactory;
            _containerIndex = containerIndex;

            SubscribeToViewEvents();
        }
        
        public void InitializeContainer(int wordLength)
        {
            var letterSlots = _wordContainerFactory.CreateLetterSlots(_view.LetterSlotPrefab, _view.transform, wordLength);

            var dependencies = _wordContainerDependenciesFactory.Create(letterSlots, _view.transform, _containerIndex);

            _dataModel = dependencies.Item1;
            _slotHandler = dependencies.Item2;
            _placeholderHelper = dependencies.Item3;
            _dropPlacementHelper = dependencies.Item4;
            _clusterTracker = dependencies.Item5;
        }
        
        private void SubscribeToViewEvents()
        {
            _view.OnClusterDropped
                .Subscribe(OnDrop)
                .AddTo(_disposable); 
            
            _view.OnTryDrop
                .Subscribe(tuple =>
                {
                    var (cluster, eventData, tcs) = tuple;
                    var result = TryDrop(cluster, eventData);
                    tcs.TrySetResult(result);
                })
                .AddTo(_disposable);
        }
        
        public void ShowPlaceholder(UIClusterElementView cluster, int startIndex) =>
            _placeholderHelper.ShowPlaceholder(cluster, startIndex);
        
        public void ClearPlaceholder() =>
            _placeholderHelper.ClearPlaceholder();

        public int CalculateSlotIndexFromPosition(Vector2 localPosition) => 
            _dropPlacementHelper.CalculateSlotIndexFromPosition(localPosition);

        public void ReturnClusterToPosition(UIClusterElementView cluster) =>
            _clusterTracker.ReturnCluster(cluster);

        public void ReleaseSlotsForCluster(UIClusterElementView cluster) =>
            _slotHandler.ReleaseSlots(cluster);

        public void ClearBuffers() =>
            _dataModel.BufferSlots.Clear();

        public Dictionary<UIClusterElementPresenter, int> GetPlacedClusters()
        {
            var result = new Dictionary<UIClusterElementPresenter, int>();

            foreach (var kvp in _dataModel.ClusterStartIndices)
            {
                var clusterView = kvp.Key;
                if (_dataModel.PlacedClusters.ContainsKey(clusterView))
                {
                    result[clusterView.Presenter] = kvp.Value;
                }
            }

            return result;
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
            _dropPlacementHelper.TryDropCluster(cluster, eventData, _view.transform);

        public void Destroy()
        {
            _disposable?.Dispose();
            Object.Destroy(_view.gameObject);
        }
    }
}