using System;
using Cysharp.Threading.Tasks;
using UI.Abstract;
using UI.Gameplay.Elements;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Gameplay.BottomPanel
{
    public class UIBottomPanelView : UIView, IClusterDropZone
    {
        [SerializeField] private ClusterPanelSettings _clusterPanelSettings;
        
        public ClusterPanelSettings ClusterPanelSettings => _clusterPanelSettings;
        
        private readonly Subject<PointerEventData> _onClusterDropped = new();

        private readonly Subject<(UIClusterElementView cluster, PointerEventData eventData, UniTaskCompletionSource<bool> tcs)>
            _onTryDrop = new();
        
        public IObservable<PointerEventData> OnClusterDropped => _onClusterDropped;

        public IObservable<(UIClusterElementView cluster, PointerEventData eventData, UniTaskCompletionSource<bool> tcs)>
            OnTryDrop => _onTryDrop;
        
        public void OnDrop(PointerEventData eventData) =>
            _onClusterDropped.OnNext(eventData);

        public async UniTask<bool> TryDrop(UIClusterElementView cluster, PointerEventData eventData)
        {
            Debug.Log("UIBottomPanelView TryDrop");
            
            var taskSource = new UniTaskCompletionSource<bool>();
            _onTryDrop.OnNext((cluster, eventData, taskSource));
            return await taskSource.Task;
        }
        
        private void OnDestroy()
        {
            _onTryDrop?.OnCompleted();
            _onClusterDropped?.OnCompleted();
        }
    }
}