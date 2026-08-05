using System;
using Cysharp.Threading.Tasks;
using UI.Abstract;
using UI.Gameplay.WordContainers;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Gameplay.Elements
{
    public sealed class UIWordContainerView : UIView, IClusterDropZone
    {
        [SerializeField] private UILetterSlotView _letterSlotPrefab;
        
        private UIWordContainerPresenter _presenter;

        private readonly Subject<(UIClusterElementView cluster, PointerEventData eventData, UniTaskCompletionSource<bool> tcs)>
            _onTryDrop = new();

        public UILetterSlotView LetterSlotPrefab => _letterSlotPrefab;

        public UIWordContainerPresenter Presenter => _presenter;

        public IObservable<(UIClusterElementView cluster, PointerEventData eventData, UniTaskCompletionSource<bool> tcs)>
            OnTryDrop => _onTryDrop;

        public void Initialize(UIWordContainerPresenter presenter) =>
            _presenter = presenter;

        public async UniTask<bool> TryDrop(UIClusterElementView cluster, PointerEventData eventData)
        {
            var taskSource = new UniTaskCompletionSource<bool>();
            _onTryDrop.OnNext((cluster, eventData, taskSource));
            return await taskSource.Task;
        }

        private void OnDestroy()
        {
            _onTryDrop?.OnCompleted();
        }
    }
}