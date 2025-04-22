using System;
using UI.Abstract;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Gameplay.Elements
{
    public class UIClusterElementView : UIView, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private Transform _frame;
        
        private UIClusterElementPresenter _presenter;

        private readonly Subject<PointerEventData> _onDragStarted = new();
        private readonly Subject<Vector2> _onDragging = new();
        private readonly Subject<PointerEventData> _onDragEnded = new();

        public UIClusterElementPresenter Presenter => _presenter;
        public IObservable<PointerEventData> OnDragStarted => _onDragStarted.AsObservable();
        public IObservable<Vector2> OnDragging => _onDragging.AsObservable();
        public IObservable<PointerEventData> OnDragEnded => _onDragEnded.AsObservable();

        public void Initialize(UIClusterElementPresenter presenter) =>
            _presenter = presenter;
        
        public void OnBeginDrag(PointerEventData eventData) =>
            _onDragStarted.OnNext(eventData); 
        
        public void OnDrag(PointerEventData eventData) =>
            _onDragging.OnNext(eventData.position);
        
        public void OnEndDrag(PointerEventData eventData) =>
            _onDragEnded.OnNext(eventData);

        public void UpdateFrame()
            => _frame.SetAsLastSibling();
        
        private void OnDestroy() =>
            _presenter.Clear();
    }
}
