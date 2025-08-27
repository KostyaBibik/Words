using DataBase.Models;
using UI.Abstract;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Gameplay.Elements
{
    public class UIClusterElementPresenter : UIPresenter<UIClusterElementView>
    {
        private ClusterElementData _dataModel;

        private Transform _viewTransform;
        private RectTransform _viewRectTransform;
        
        private readonly CompositeDisposable _disposable = new();
        
        public UIClusterElementPresenter(UIClusterElementView view) : base(view)
        {
        }

        public new void Initialize()
        {
            _dataModel = new ClusterElementData
            {
                OriginalParent = _view.transform.parent
            };

            _viewTransform = _view.transform;
            _viewRectTransform = (RectTransform)_viewTransform;

            SubscribeToViewEvents();
        }

        private void SubscribeToViewEvents()
        {
            _view
                .OnDragStarted
                .Subscribe(OnBeginDrag)
                .AddTo(_disposable);
        }
        
        public void AddLetter(UILetterView letter) =>
            _dataModel.Letters.Add(letter);
        
        public void SetContainer(UIWordContainerView container) =>
            _dataModel.Container = container;

        public Transform GetOriginalParent() =>
            _dataModel.OriginalParent;  
        
        public UIWordContainerView GetContainer() =>
            _dataModel.Container;
        
        public int GetGrabbedLetterIndex() =>
            _dataModel.GrabbedLetterIndex; 
        
        public int GetLettersCount() =>
            _dataModel.LetterCount;
        
        public void SetOrderInWord(int order) =>
            _dataModel.Data.orderInWord = order;
        
        public void SetWordGroupIndex(int index) =>
            _dataModel.Data.wordGroupIndex = index;
        
        public void SetData(ClusterData data) =>
            _dataModel.Data = data;

        public ClusterData GetData() =>
             _dataModel.Data;
        
        public async void ReturnToOriginalPosition()
        {
            _viewTransform.SetParent(_dataModel.OriginalParent);
            _viewTransform.localPosition = Vector3.zero;
            _viewTransform.SetSiblingIndex(_dataModel.OriginalSiblingIndex);
            await _view.Show();
        }

        public void Clear() =>
            _disposable?.Dispose();

        private void OnBeginDrag(PointerEventData eventData)
        {
            _dataModel.OriginalParent = _viewTransform.parent;
            _dataModel.OriginalSiblingIndex = _viewTransform.GetSiblingIndex();

            CalculateDragOffset(eventData);

            _dataModel.GrabbedLetterIndex = CalculateGrabbedLetterIndex();
        }

        private void CalculateDragOffset(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _viewRectTransform,
                eventData.position,
                eventData.pressEventCamera,
                out _dataModel.DragOffset);
        }

        private int CalculateGrabbedLetterIndex()
        {
            var rect = _view.GetComponent<RectTransform>().rect;
            var localX = _dataModel.DragOffset.x + rect.width / 2f;
            var letterCount = _dataModel.LetterCount;
            var letterWidth = rect.width / letterCount;

            return Mathf.Clamp(Mathf.FloorToInt(localX / letterWidth), 0, letterCount - 1);
        }
    }
}